using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Errant.src.World.Generation;
using Microsoft.Xna.Framework;

namespace Errant.src.World
{
	public class Chunk
	{
		private static readonly Dictionary<byte, Vector2> edgeOffsets = new Dictionary<byte, Vector2> {
			{ 1, new Vector2(1, 0) },
			{ 2, new Vector2(0, 1)  },
			{ 4, new Vector2(-1, 0)  },
			{ 8, new Vector2(0, -1) },
		};

		private static readonly Dictionary<byte, Vector2> cornerOffsets = new Dictionary<byte, Vector2> {
			{ 1, new Vector2(1, 1)  },
			{ 2, new Vector2(-1, 1) },
			{ 4, new Vector2(-1, -1)},
			{ 8, new Vector2(1, -1) },
		};
		
		private PersistentTile[] tileData;
		private ActiveTile[] activeTiles;

		public Chunk(GenerationData genData, int chunkX, int chunkY, int globalWidth) {
			activeTiles = new ActiveTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
			tileData = new PersistentTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
			
			int localTileIndex = 0;
			int globalTileIndex;

			int startingTileY = (chunkY * Config.CHUNK_SIZE);
			int startingTileX = (chunkX * Config.CHUNK_SIZE);

			for (int y = startingTileY; y < (startingTileY + Config.CHUNK_SIZE); y++) {
				for (int x = startingTileX; x < (startingTileX + Config.CHUNK_SIZE); x++) {
					globalTileIndex = (y * globalWidth) + x;
					tileData[localTileIndex] = new PersistentTile(genData.pointData[globalTileIndex]);
					localTileIndex++;
				}
			}
		}

		public Chunk(PersistentTile[] tiles) {
			tileData = tiles;
			activeTiles = new ActiveTile[tileData.Length];
		}

		public Chunk() {
			activeTiles = new ActiveTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
			tileData = new PersistentTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];

			for (int i = 0; i < tileData.Length; i++) {
				tileData[i] = new PersistentTile();
			}
		}

		public void Load() {
			
			int nG, nGX, nGY;
            ushort nGId, cGId;
            int cGPriority, nGPriority;
			int x, y;
			int i;
			
			for (x = 0; x < tileData.Length; x++) {
				activeTiles[x] = new ActiveTile(tileData[x]);
			}
			
            for (y = 0; y < Config.CHUNK_SIZE; y++) {
                for (x = 0; x < Config.CHUNK_SIZE; x++) {
	                
                    i = (y * Config.CHUNK_SIZE) + x;
                    cGId = tileData[i].GroundTileId;
                    cGPriority = Array.IndexOf(GroundIds.priorities, cGId);

                    foreach (KeyValuePair<byte, Vector2> entry in edgeOffsets) {
                        nGX = x + (int)entry.Value.X;
                        nGY = y + (int)entry.Value.Y;
	                    if (nGY >= 0 && nGX >= 0 && nGY < Config.CHUNK_SIZE && nGX < Config.CHUNK_SIZE) {
		                    nG = (nGY * Config.CHUNK_SIZE) + nGX;
		                    nGId = tileData[nG].GroundTileId;
		                    nGPriority = Array.IndexOf(GroundIds.priorities, nGId);
		                    if (nGPriority < cGPriority) {
			                    activeTiles[nG].IncrementEdgeTransition(cGId, entry.Key);
		                    }
	                    }
                    }

                    foreach (KeyValuePair<byte, Vector2> entry in cornerOffsets) {
                        nGX = x + (int)entry.Value.X;
                        nGY = y + (int)entry.Value.Y;
	                    if (nGY >= 0 && nGX >= 0 && nGY < Config.CHUNK_SIZE && nGX < Config.CHUNK_SIZE) {
		                    nG = (nGY * Config.CHUNK_SIZE) + nGX;
		                    nGId = tileData[nG].GroundTileId;
		                    nGPriority = Array.IndexOf(GroundIds.priorities, nGId);
		                    if (nGPriority < cGPriority) {
			                    activeTiles[nG].IncrementCornerTransition(cGId, entry.Key);
		                    }
	                    }
                    }
                }
            }

            foreach (ActiveTile tile in activeTiles) {
                tile.PrepTransitionData();
            }
		}
		
		public ActiveTile[] GetActiveTiles() {
			return activeTiles;
		}

		public PersistentTile[] GetTiles() {
			return tileData;
		}
	}
}
