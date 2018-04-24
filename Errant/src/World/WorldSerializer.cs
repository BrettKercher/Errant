using System;
using System.IO;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Errant.src.World {
    public class WorldSerializer {

        private BinaryWriter writer;
        private BinaryReader reader;
        private FileStream fileStream;

        private const string fileExtension = ".world";
        private const int bytesPerTile = 3; //Number of bytes used to repesent a single tile
        private const int numSections = 10;

        enum Masks {
            None = 0,
            GroundIdIsShort = 1,
            WallPresent = 1 << 1,
            WallIdIsShort = 1 << 2,
            OverPresent = 1 << 3,
            OverIdIsShort = 1 << 4,
            ObjectPresent = 1 << 5,
            ObjectIdIsShort = 1 << 6
        }

        public void Serialize(WorldData world) {

            WorldHeader header = world.GetHeader();
            
            string dir = Path.Combine(Config.WorldSaveDirectory, header.name);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            fileStream = new FileStream(Path.Combine(dir, header.name + fileExtension), FileMode.Create, FileAccess.Write);
            writer = new BinaryWriter(fileStream);
            int[] sectionPointers = new int[numSections];

            sectionPointers[0] = BlockOutSectionPointer(world);
            sectionPointers[1] = SerializeHeader(world);
            sectionPointers[2] = SerializeChunks(world);
            SerializeSectionPointers(world, sectionPointers);

            writer.Close();
            fileStream.Close();
        }

        private int BlockOutSectionPointer(WorldData world) {
            writer.Write(world.GetVersion());
            writer.Write((byte) numSections);
            for (int i = 0; i < numSections; i++) {
                writer.Write(0);
            }

            return (int) writer.BaseStream.Position;
        }

        private int SerializeHeader(WorldData world) {
            WorldHeader header = world.GetHeader();
            writer.Write(header.name);
            writer.Write(header.width);
            writer.Write(header.height);
            writer.Write(header.spawnArea.X);
            writer.Write(header.spawnArea.Y);
            
            return (int) writer.BaseStream.Position;
        }

        private int SerializeChunks(WorldData world) {
            byte b; //represents 8 booleans compacted into a single byte
            byte[] tileData; //byte array to hold tile data
            PersistentTile tile; //Cached version of the current tile
            int byteIndex; //Current index into the tileData array
            
            var chunks = world.GetChunks();

            writer.Write(chunks.Length);

            for (int i = 0; i < chunks.Length; i++) {
                var tiles = chunks[i].GetTiles();
                for (int j = 0; j < tiles.Length; j++) {
                    b = 0;
                    byteIndex = 1;
                    tile = tiles[j];
                    tileData = new byte[bytesPerTile];
                    tileData[byteIndex] = (byte) tile.GroundTileId;
                    byteIndex++;
                    if (tile.ObjectTileId > 0) {
                        b |= (byte) Masks.WallPresent;
                        tileData[byteIndex] = (byte) tile.ObjectTileId;
                        byteIndex++;
                    }
                    tileData[0] = b;
                    writer.Write(tileData, 0, byteIndex);
                }
            }
            
            
            return (int) writer.BaseStream.Position;
        }

        private void SerializeSectionPointers(WorldData world, int[] pointers) {
            writer.BaseStream.Position = 0L;

            WorldHeader header = world.GetHeader();
            writer.Write(world.GetVersion());
            writer.Write((byte) pointers.Length);

            for (int i = 0; i < pointers.Length; i++) {
                writer.Write(pointers[i]);
            }
        }

        public WorldData Deserialize(string worldName) {
            WorldData worldData = new WorldData();

            string worldFilePath = Path.Combine(Config.WorldSaveDirectory, worldName, worldName + fileExtension);
            
            if (!File.Exists(worldFilePath)) {
                System.Diagnostics.Debug.WriteLine("Attempted to load a World that doesn't Exist!");
                return null;
            }

            using (fileStream = new FileStream(worldFilePath, FileMode.Open)) {
                using (reader = new BinaryReader(fileStream)) {
                    try {
                        int[] array;

                        DeserializeSectionPointers(worldData, out array);
                        if (reader.BaseStream.Position != array[0]) {
                            throw new Exception("Invalid world file format - after reading section pointers!");
                        }

                        DeserializeWorldHeader(worldData);
                        if (reader.BaseStream.Position != array[1]) {
                            throw new Exception("Invalid world file format - after reading world header!");
                        }

                        DeserializeWorldChunks(worldData);
                        if (reader.BaseStream.Position != array[2]) {
                            throw new Exception("Invalid world file format - after reading world tiles!");
                        }
                    }
                    catch (Exception e) {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    finally {
                        reader.Close();
                        fileStream.Close();
                    }
                }
            }

            return worldData;
        }

        private void DeserializeSectionPointers(WorldData world, out int[] sectionPointers) {
            world.SetVersion(reader.ReadString());
            int sections = reader.ReadByte();

            sectionPointers = new int[sections];

            for (int i = 0; i < sections; i++) {
                sectionPointers[i] = reader.ReadInt32();
            }
        }

        private void DeserializeWorldHeader(WorldData worldData) {
            worldData.SetName(reader.ReadString());
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            int spawnX = reader.ReadInt32();
            int spawnY = reader.ReadInt32();

            WorldHeader header = new WorldHeader();
            header.width = width;
            header.height = height;
            header.spawnArea = new Rectangle(spawnX, spawnY, Config.SPAWN_AREA_SIZE, Config.SPAWN_AREA_SIZE);
            worldData.SetHeader(header);
        }

        private void DeserializeWorldChunks(WorldData worldData) {
            byte b; //represents 8 booleans compacted into a single byte

            int numChunks = reader.ReadInt32();
            
            Chunk[] chunks = new Chunk[numChunks];
            PersistentTile[] tiles;

            for (int i = 0; i < numChunks; i++) {
                tiles = new PersistentTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
                for (int j = 0; j < tiles.Length; j++) {
                    tiles[j] = new PersistentTile();

                    b = reader.ReadByte();
                    tiles[j].GroundTileId = reader.ReadByte();

                    if ((b & (byte) Masks.WallPresent) == (byte) Masks.WallPresent) {
                        tiles[j].ObjectTileId = reader.ReadByte();
                    }
                }
                chunks[i] = new Chunk(tiles);
            }
            
            worldData.SetChunks(chunks);
        }
    }
}