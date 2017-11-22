using Errant.src.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World
{
	class WorldData
	{
		int width;		// Width of the world in chunks
		int height;     // Height of the world in chunks
		Chunk[] chunks;

		public WorldData(GenerationData genData)
		{
			int chunkX, chunkY;
			int startTileX, startTileY;
			int pointIndex, tileIndex;
			int i, j, k;
			Tile[] tiles;

			width = genData.width / Config.CHUNK_SIZE;
			height = genData.height / Config.CHUNK_SIZE;
			chunks = new Chunk[width * height];

			for(i = 0; i < chunks.Length; i++) {
				chunks[i] = new Chunk();
				tiles = chunks[i].GetTiles();
				chunkX = i % width;
				chunkY = i / width;
				startTileX = chunkX * Config.CHUNK_SIZE;
				startTileY = chunkY * Config.CHUNK_SIZE;
				tileIndex = 0;

				for (j = startTileY; j < startTileY + Config.CHUNK_SIZE; j++) {
					for (k = startTileX; k < startTileX + Config.CHUNK_SIZE; k++) {
						pointIndex = (j * genData.width) + k;
						tiles[tileIndex] = new Tile(genData.pointData[pointIndex]);
						tileIndex++;
					}
				}
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

		public Chunk GetChunk(int chunkIndex) {
			if(chunkIndex >= 0 && chunkIndex < chunks.Length) {
				return chunks[chunkIndex];
			}
			return null;
		}
	}
}
