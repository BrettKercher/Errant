using Errant.src.Graphics;
using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Errant.src.World {

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
		private WorldSerializer serializer;
		private WorldGenerator generator;

        private TextureAtlas groundTileAtlas;
        private readonly int TILE_ROWS = 32;
        private readonly int TILE_COLUMNS = 32;

        public WorldManager() {
			generator = new WorldGenerator();
	        serializer = new WorldSerializer();
		}

		public void LoadContent(ContentManager content) {
            Texture2D tileMap = content.Load<Texture2D>("sprites/tileMap");
            groundTileAtlas = new TextureAtlas(tileMap, TILE_COLUMNS, TILE_ROWS, Config.TILE_SIZE, Config.TILE_SIZE);
        }

		public int GetWidthInChunks() {
			return worldData.GetWidth() / Config.CHUNK_SIZE;
		}

		public int GetHeightInChunks() {
			return worldData.GetHeight() / Config.CHUNK_SIZE;
		}

		public void GenerateWorld(GenerationSettings settings, BackgroundWorker worker = null) {
			GenerationData genData = generator.Generate(settings, worker);
			worldData = new WorldData(genData);
//			SaveWorld();
//			LoadWorld();
		}

		public void SaveWorld() {
			serializer.Serialize(worldData);
		}

		public void LoadWorld() {
			worldData = serializer.Deserialize();
		}

		public void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch, int origin, int viewDistance) {
			int chunkIndex, y, x;

			int originX = origin % GetWidthInChunks();
			int originY = origin / GetWidthInChunks();

			for (y = originY - viewDistance; y <= originY + viewDistance; y++) {
				if (y < 0 || y >= GetHeightInChunks()) { continue; }
				for (x = originX - viewDistance; x <= originX + viewDistance; x++) {
					if(x < 0 || x >= GetWidthInChunks()) { continue; }
					chunkIndex = (y * GetWidthInChunks()) + x;
                    DrawChunk(gameTime, spriteBatch, chunkIndex);
				}
			}
		}

		private void DrawChunk(GameTime gameTime, SpriteBatch spriteBatch, int chunkIndex) {

			Chunk chunk = worldData.LoadChunk(chunkIndex);
			if (chunk == null) {
				return;
			}

			ActiveTile[] tiles = chunk.GetTiles();

			int x, y;
			int xPos, yPos;

			int chunkX = chunkIndex % GetWidthInChunks();
			int chunkY = chunkIndex / GetWidthInChunks();

			int chunkOffsetX = chunkX * Config.CHUNK_SIZE * Config.TILE_SIZE;
			int chunkOffsetY = chunkY * Config.CHUNK_SIZE * Config.TILE_SIZE;

			for (int i = 0; i < tiles.Length; i++) {
				x = i % Config.CHUNK_SIZE;
				y = i / Config.CHUNK_SIZE;

				xPos = chunkOffsetX + (x * Config.TILE_SIZE);
				yPos = chunkOffsetY + (y * Config.TILE_SIZE);
                tiles[i].DrawGround(spriteBatch, groundTileAtlas, xPos, yPos);
                tiles[i].DrawObject(spriteBatch, xPos, yPos);
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

            return (chunkY * GetWidthInChunks()) + chunkX;
        }
		
		/// <summary>
		/// Removes the object at the given location
		/// </summary>
		/// <param name="tileX">Global x tile position of the object to remove</param>
		/// <param name="tileY">Global y tile position of the object to remove</param>
		public void RemoveObjectAt(int tileX, int tileY) {
			ActiveTile tile = worldData.GetActiveTile(tileX, tileY);
			tile.ClearObject();
		}
		
		/// <summary>
		/// Place the given object At the given location
		/// </summary>
		/// <param name="objId">Id of the object to place</param>
		/// <param name="tileX">Global x tile position of the object to remove</param>
		/// <param name="tileY">Global y tile position of the object to remove</param>
		public void PlaceObjectAt(ushort objId, int tileX, int tileY) {
			ActiveTile tile = worldData.GetActiveTile(tileX, tileY);
			tile.PlaceObject(objId);
		}

        public void PrintTileData(int tileX, int tileY) {
            ActiveTile tile = worldData.GetActiveTile((tileY * worldData.GetWidth()) + tileX);

            tile.PrintDebugInfo();
        }
    }
}
