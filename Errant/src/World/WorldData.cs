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

	public struct WorldHeader {
		public string name;
		public string versionNumber;
		public int width;		// Width of the world in tiles
		public int height;     // Height of the world in tiles
	}
	
	public class WorldData {
		
		private WorldHeader header;
        private PersistentTile[] tileData;
        private Dictionary<int, Chunk> loadedChunks;

        private readonly Dictionary<byte, Vector2> edgeOffsets = new Dictionary<byte, Vector2> {
                { 1, new Vector2(1, 0) },
                { 2, new Vector2(0, 1)  },
                { 4, new Vector2(-1, 0)  },
                { 8, new Vector2(0, -1) },
            };

        private readonly Dictionary<byte, Vector2> cornerOffsets = new Dictionary<byte, Vector2> {
                { 1, new Vector2(1, 1)  },
                { 2, new Vector2(-1, 1) },
                { 4, new Vector2(-1, -1)},
                { 8, new Vector2(1, -1) },
            };

        public WorldData(GenerationData genData) {
            int i;
            int numTiles = genData.pointData.Length;
            tileData = new PersistentTile[numTiles];
            loadedChunks = new Dictionary<int, Chunk>();

            header.width = genData.width;
	        header.height = genData.height;

            for (i = 0; i < numTiles; i++) {
                tileData[i] = new PersistentTile(genData.pointData[i]);
            }
        }

		public WorldData(WorldHeader _header) {
			header = _header;
			loadedChunks = new Dictionary<int, Chunk>();
			int numTiles = header.width * header.height;
			tileData = new PersistentTile[numTiles];
			
			for (int i = 0; i < numTiles; i++) {
				tileData[i] = new PersistentTile();
			}
		}
	    
	    public WorldData() {
	        loadedChunks = new Dictionary<int, Chunk>();
	    }

		public WorldHeader GetWorldHeader() {
			return header;
		}

	    public PersistentTile[] GetTileData() {
	        return tileData;
	    }

		public int GetWidth() {
			return header.width;
		}

		public int GetHeight() {
			return header.height;
		}

		public string GetName() {
			return header.name;
		}

		public void SetName(string name) {
			header.name = name;
		}

		public string Version() {
			return header.versionNumber;
		}

		public void SetVersion(string version) {
			header.versionNumber = version;
		}

	    public void SetDimensions(int w, int h) {
		    header.width = w;
		    header.height = h;
	        int i;
	        int numTiles = w * h;
	        tileData = new PersistentTile[numTiles];

	        for (i = 0; i < numTiles; i++) {
	            tileData[i] = new PersistentTile();
	        }
	    }

        public PersistentTile GetPersistentTile(int tileIndex) {
            return tileData[tileIndex];
        }

	    public ActiveTile GetActiveTile(int tileX, int tileY) {
	        int localTileX = tileX % Config.CHUNK_SIZE;
	        int localTileY = tileY % Config.CHUNK_SIZE;

	        int chunkX = tileX / Config.CHUNK_SIZE;
	        int chunkY = tileY / Config.CHUNK_SIZE;

	        int chunkIndex = (chunkY * (header.width / Config.CHUNK_SIZE)) + chunkX;

	        Chunk chunk;
	        if(!loadedChunks.TryGetValue(chunkIndex, out chunk)) {
	            return null;
	        }

	        return chunk.GetTiles()[(localTileY * Config.CHUNK_SIZE) + localTileX];
	    }

        public ActiveTile GetActiveTile(int tileIndex) {
            int tileX = tileIndex % header.width;
            int tileY = tileIndex / header.width;

            return GetActiveTile(tileX, tileY);
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
	        int width = header.width;
	        int height = header.height;
            
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
