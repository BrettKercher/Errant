using Errant.src.Graphics;
using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Errant.src.World {

	struct DrawParams {
		public Color color;
		public Texture2D texture;
	}

	class WorldManager {

		public enum DRAW_MODE {
			Shape = 0,
			Elevation = 1,
			Temperature = 2,
			Moisture = 3,
			Biome = 4,
			Debug1 = 8,
			Debug2 = 9,
		}

		private WorldData worldData;
		private WorldGenerator generator;

        private TileAtlas tileAtlas;
        private readonly int ATLAS_ROWS = 15;
        private readonly int ATLAS_COLUMNS = 15;

// 		private Texture2D tileTexture;

		public WorldManager() {
			generator = new WorldGenerator();
		}

		public void LoadContent(ContentManager content) {
            Texture2D tileMap = content.Load<Texture2D>("sprites/tileMap");
            tileAtlas = new TileAtlas(tileMap, ATLAS_COLUMNS, ATLAS_ROWS, Config.TILE_SIZE, Config.TILE_SIZE);
		}

		public int GetWidth() {
			return worldData.GetWidth();
		}

		public int GetHeight() {
			return worldData.GetHeight();
		}

		public void GenerateWorld(GenerationSettings settings, BackgroundWorker worker = null) {
			GenerationData genData = generator.Generate(settings, worker);
			worldData = new WorldData(genData);
		}

		public void SaveWorld() { }

		public void LoadWorld() { }

		public void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch, int origin, int viewDistance) {
			int chunkIndex, y, x;

			int originX = origin % GetWidth();
			int originY = origin / GetWidth();

			for (y = originY - viewDistance; y <= originY + viewDistance; y++) {
				if (y < 0 || y >= GetHeight()) { continue; }
				for (x = originX - viewDistance; x <= originX + viewDistance; x++) {
					if(x < 0 || x >= GetWidth()) { continue; }
					chunkIndex = (y * GetWidth()) + x;
					DrawChunk(gameTime, spriteBatch, chunkIndex);
				}
			}
		}

		private void DrawChunk(GameTime gameTime, SpriteBatch spriteBatch, int chunkIndex) {
			Chunk chunk = worldData.GetChunk(chunkIndex);
			if (chunk == null) {
				return;
			}

			Tile[] tiles = chunk.GetTiles();

			int x, y;
			int xPos, yPos;

			int chunkX = chunkIndex % GetWidth();
			int chunkY = chunkIndex / GetWidth();

			int chunkOffsetX = chunkX * Config.CHUNK_SIZE * Config.TILE_SIZE;
			int chunkOffsetY = chunkY * Config.CHUNK_SIZE * Config.TILE_SIZE;

			for (int i = 0; i < tiles.Length; i++) {
				x = i % Config.CHUNK_SIZE;
				y = i / Config.CHUNK_SIZE;

				xPos = chunkOffsetX + (x * Config.TILE_SIZE);
				yPos = chunkOffsetY + (y * Config.TILE_SIZE);
                int textureRegionIndex = (tiles[i].TextureRegionRow * ATLAS_COLUMNS) + 14;
                TextureRegion2D textureRegion = tileAtlas.GetTextureRegion(textureRegionIndex);
                spriteBatch.Draw(textureRegion.Texture, new Vector2(xPos, yPos), textureRegion.SourceRectangle, Color.White);
			}
		}


        /// <summary>
        /// Get the index of the chunk currently occupied by the passed in position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Chunk Index</returns>
        public int GetContainingChunkIndex(Vector2 position) {

            int chunkPixelSize = (Config.TILE_SIZE * Config.CHUNK_SIZE);

            int chunkX = (int)Math.Floor(position.X / chunkPixelSize);
            int chunkY = (int)Math.Floor(position.Y / chunkPixelSize);

            return (chunkY * GetWidth()) + chunkX;
        }
    }
}
