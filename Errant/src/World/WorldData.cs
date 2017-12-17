using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World
{
	class WorldData
	{
		int width;		// Width of the world in chunks
		int height;     // Height of the world in chunks
        PersistentTile[] tileData;

        private Dictionary<int, Chunk> loadedChunks;

        Dictionary<byte, Vector2> edgeOffsets = new Dictionary<byte, Vector2>() {
                { 1, new Vector2(1, 0) },
                { 2, new Vector2(0, 1)  },
                { 4, new Vector2(-1, 0)  },
                { 8, new Vector2(0, -1) },
            };

        Dictionary<byte, Vector2> cornerOffsets = new Dictionary<byte, Vector2>() {
                { 1, new Vector2(1, 1)  },
                { 2, new Vector2(-1, 1) },
                { 4, new Vector2(-1, -1)},
                { 8, new Vector2(1, -1) },
            };

        public WorldData(GenerationData genData)
		{
            int i;
            int numTiles = genData.pointData.Length;
            tileData = new PersistentTile[numTiles];
            loadedChunks = new Dictionary<int, Chunk>();

            width = genData.width;
            height = genData.height;

            for (i = 0; i < numTiles; i++) {
                tileData[i] = new PersistentTile(genData.pointData[i]);
            }
        }

		public int GetWidth()
		{
			return width;
		}

		public int GetHeight()
		{
			return height;
		}

        public PersistentTile GetPersistentTile(int tileIndex) {
            return tileData[tileIndex];
        }

        public ActiveTile GetActiveTile(int tileIndex) {
            int tileX = tileIndex % width;
            int tileY = tileIndex / width;

            int localTileX = tileX % Config.CHUNK_SIZE;
            int localTileY = tileY % Config.CHUNK_SIZE;

            int chunkX = tileX / Config.CHUNK_SIZE;
            int chunkY = tileY / Config.CHUNK_SIZE;

            int chunkIndex = (chunkY * (width / Config.CHUNK_SIZE)) + chunkX;

            Chunk chunk;
            if(!loadedChunks.TryGetValue(chunkIndex, out chunk)) {
                return null;
            }

            return chunk.GetTiles()[(localTileY * Config.CHUNK_SIZE) + localTileX];
        }

        public Chunk LoadChunk(int chunkIndex) {
            Chunk chunk;

            if (loadedChunks.TryGetValue(chunkIndex, out chunk)) {
                //Chunk already loaded
                return chunk;
            }

            chunk = new Chunk();

            int i, j;
            int chunkX, chunkY;
			int startTileX, startTileY;
			int pointIndex, tileIndex;
			ActiveTile[] tiles;
            
            tiles = chunk.GetTiles();
            chunkX = chunkIndex % (width / Config.CHUNK_SIZE);
            chunkY = chunkIndex / (width / Config.CHUNK_SIZE);
            startTileX = chunkX * Config.CHUNK_SIZE;
            startTileY = chunkY * Config.CHUNK_SIZE;
            tileIndex = 0;

            for (j = startTileY; j < startTileY + Config.CHUNK_SIZE; j++) {
                for (i = startTileX; i < startTileX + Config.CHUNK_SIZE; i++) {
                    pointIndex = (j * width) + i;
                    tiles[tileIndex] = new ActiveTile(tileData[pointIndex]);
                    tileIndex++;
                }
            }

            // Calculate Transitions
            int nG, nGX, nGY;
            int nL, nLX, nLY;
            ushort nGId, cGId;
            int cGPriority, nGPriority;
            for (j = startTileY - 1; j <= startTileY + Config.CHUNK_SIZE; j++) {
                for (i = startTileX - 1; i <= startTileX + Config.CHUNK_SIZE; i++) {
                    if (j < 0 || j >= height || i < 0 || i >= width) { continue; }
                    pointIndex = (j * width) + i;
                    cGId = tileData[pointIndex].GroundTileId;
                    cGPriority = Array.IndexOf(GroundIds.priorities, cGId);

                    foreach (KeyValuePair<byte, Vector2> entry in edgeOffsets) {
                        nGX = i + (int)entry.Value.X;
                        nGY = j + (int)entry.Value.Y;
                        if (nGY >= 0 && nGX >= 0 && nGY < height && nGX < width) {
                            nG = (nGY * width) + nGX;
                            nGId = tileData[nG].GroundTileId;
                            nGPriority = Array.IndexOf(GroundIds.priorities, nGId);
                            nLX = nGX - startTileX;
                            nLY = nGY - startTileY;
                            if (nLX >= 0 && nLX < Config.CHUNK_SIZE && nLY >= 0 && nLY < Config.CHUNK_SIZE && nGPriority < cGPriority) {
                                nL = (nLY * Config.CHUNK_SIZE) + nLX;
                                tiles[nL].IncrementEdgeTransition(cGId, entry.Key);
                            }
                        }
                    }

                    foreach (KeyValuePair<byte, Vector2> entry in cornerOffsets) {
                        nGX = i + (int)entry.Value.X;
                        nGY = j + (int)entry.Value.Y;
                        if (nGY >= 0 && nGX >= 0 && nGY < height && nGX < width) {
                            nG = (nGY * width) + nGX;
                            nGId = tileData[nG].GroundTileId;
                            nGPriority = Array.IndexOf(GroundIds.priorities, nGId);
                            nLX = nGX - startTileX;
                            nLY = nGY - startTileY;
                            if (nLX >= 0 && nLX < Config.CHUNK_SIZE && nLY >= 0 && nLY < Config.CHUNK_SIZE && nGPriority < cGPriority) {
                                nL = (nLY * Config.CHUNK_SIZE) + nLX;
                                tiles[nL].IncrementCornerTransition(cGId, entry.Key);
                            }
                        }
                    }
                }
            }

            foreach (ActiveTile tile in tiles) {
                tile.PrepTransitionData();
            }

            loadedChunks.Add(chunkIndex, chunk);
            return chunk;
        }
    }
}
