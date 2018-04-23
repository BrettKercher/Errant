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
//        private PersistentTile[] tileData;
		private Chunk[] chunks;
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
			int chunkWidth = (genData.width / Config.CHUNK_SIZE);
			int chunkHeight = (genData.height / Config.CHUNK_SIZE);
			chunks = new Chunk[chunkWidth * chunkHeight];
			loadedChunks = new Dictionary<int, Chunk>();
			
			header.width = genData.width;
			header.height = genData.height;
			header.versionNumber = "0.0.1";

			for (int i = 0; i < (chunkWidth * chunkHeight); i++) {
				chunks[i] = new Chunk(genData, i % chunkWidth, i / chunkWidth, header.width);
			}
		}

		// Generates an empty world based on the given header
		public WorldData(WorldHeader _header) {
			header = _header;

			int chunkWidth = (header.width / Config.CHUNK_SIZE);
			int chunkHeight = (header.height / Config.CHUNK_SIZE);
			chunks = new Chunk[chunkWidth * chunkHeight];
			loadedChunks = new Dictionary<int, Chunk>();
			
			for (int i = 0; i < (chunkWidth * chunkHeight); i++) {
				chunks[i] = new Chunk();
			}
		}
	    
	    public WorldData() {
	        loadedChunks = new Dictionary<int, Chunk>();
	    }

		public WorldHeader GetWorldHeader() {
			return header;
		}

//	    public PersistentTile[] GetTileData() {
//	        return tileData;
//	    }

		public Chunk[] GetChunks() {
			return chunks;
		}

		public void SetChunks(Chunk[] value) {
			chunks = value;
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

	        return chunk.GetActiveTiles()[(localTileY * Config.CHUNK_SIZE) + localTileX];
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

	        chunk = chunks[chunkIndex];
	        
	        chunk.Load();

            loadedChunks.Add(chunkIndex, chunk);
            return chunk;
        }
    }
}
