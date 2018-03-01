using System;
using System.IO;

namespace Errant.src.World {
    public class WorldSerializer {

        private BinaryWriter writer;
        private BinaryReader reader;
        private FileStream fileStream;

        private const string fileExtension = ".world";
        private const string saveLocation = "C:/Worlds/";
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
            if (!Directory.Exists(saveLocation)) {
                Directory.CreateDirectory(saveLocation);
            }

            fileStream = new FileStream(saveLocation + world.name + fileExtension, FileMode.Create, FileAccess.Write);
            writer = new BinaryWriter(fileStream);
            int[] sectionPointers = new int[numSections];

            sectionPointers[0] = BlockOutSectionPointer(world);
            sectionPointers[1] = SerializeHeader(world);
            sectionPointers[2] = SerializeTiles(world);
            SerializeSectionPointers(world, sectionPointers);

            writer.Close();
            fileStream.Close();
        }

        private int BlockOutSectionPointer(WorldData world) {
            writer.Write(world.versionNumber);
            writer.Write((byte) numSections);
            for (int i = 0; i < numSections; i++) {
                writer.Write(0);
            }

            return (int) writer.BaseStream.Position;
        }

        private int SerializeHeader(WorldData world) {
            writer.Write(world.name);
            writer.Write(world.GetWidth());
            writer.Write(world.GetHeight());
            
            return (int) writer.BaseStream.Position;
        }

        private int SerializeTiles(WorldData world) {
            byte b; //represents 8 booleans compacted into a single byte
            byte[] tileData; //byte array to hold tile data
            PersistentTile tile; //Cached version of the current tile
            int byteIndex; //Current index into the tileData array

            var tiles = world.GetTileData();

            for (int i = 0; i < tiles.Length; i++) {
                b = 0;
                byteIndex = 1;
                tile = tiles[i];
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

            return (int) writer.BaseStream.Position;
        }

        private void SerializeSectionPointers(WorldData world, int[] pointers) {
            writer.BaseStream.Position = 0L;

            writer.Write(world.versionNumber);
            writer.Write((byte) pointers.Length);

            for (int i = 0; i < pointers.Length; i++) {
                writer.Write(pointers[i]);
            }
        }

        public WorldData Deserialize() {
            WorldData worldData = new WorldData();

            if (!File.Exists(saveLocation + "test" + fileExtension)) {
                System.Diagnostics.Debug.WriteLine("Attempted to load a World that doesn't Exist!");
                return null;
            }

            using (fileStream = new FileStream(saveLocation + "test" + fileExtension, FileMode.Open)) {
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

                        DeserializeWorldTiles(worldData);
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
            world.versionNumber = reader.ReadString();
            int sections = reader.ReadByte();

            sectionPointers = new int[sections];

            for (int i = 0; i < sections; i++) {
                sectionPointers[i] = reader.ReadInt32();
            }
        }

        private void DeserializeWorldHeader(WorldData worldData) {
            worldData.name = reader.ReadString();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            worldData.SetDimensions(width, height);
        }

        private void DeserializeWorldTiles(WorldData worldData) {
            byte b; //represents 8 booleans compacted into a single byte
            PersistentTile tile; //Cached version of the current tile

            for (int i = 0; i < worldData.GetWidth() * worldData.GetHeight(); i++) {
                tile = worldData.GetPersistentTile(i);

                b = reader.ReadByte();

                tile.GroundTileId = reader.ReadByte();

                if ((b & (byte) Masks.WallPresent) == (byte) Masks.WallPresent) {
                    tile.ObjectTileId = reader.ReadByte();
                }
            }
        }
    }
}