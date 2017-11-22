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

		private Texture2D tileTexture;

		public WorldManager() {
			generator = new WorldGenerator();
		}

		public void LoadContent(ContentManager content) {
			tileTexture = content.Load<Texture2D>("sprites/tile");
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
				spriteBatch.Draw(tileTexture, new Vector2(xPos, yPos), tiles[i].color);
			}
		}
	}
}
