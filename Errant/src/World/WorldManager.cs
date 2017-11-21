using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        private Dictionary<BIOME, Color> biomeColorMap = new Dictionary<BIOME, Color>() {
            { BIOME.NONE, Color.Blue },
            { BIOME.ONE, Color.White },
            { BIOME.TWO, Color.Gray },
            { BIOME.THREE, Color.Cyan },
            { BIOME.FOUR, Color.Green },
            { BIOME.FIVE, Color.SaddleBrown },
            { BIOME.SIX, Color.ForestGreen },
            { BIOME.SEVEN, Color.DarkOliveGreen },
            { BIOME.EIGHT, Color.SpringGreen },
            { BIOME.NINE, Color.LawnGreen },
            { BIOME.TEN, Color.Yellow },
            { BIOME.ELEVEN, Color.Red },
            { BIOME.TWELVE, Color.OrangeRed },
        };

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

		public void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch, DRAW_MODE drawMode) {
			int chunkIndex;
			for( int i = 0; i < GetHeight(); i++) {
				for (int j = 0; j < GetWidth(); j++) {
					chunkIndex = (i * GetWidth()) + j;
					DrawChunk(gameTime, spriteBatch, drawMode, chunkIndex);
				}
			}
		}

		private void DrawChunk(GameTime gameTime, SpriteBatch spriteBatch, DRAW_MODE drawMode, int chunkIndex) {
			Chunk chunk = worldData.GetChunk(chunkIndex);
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

			//public void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch, DRAW_MODE drawMode) { 
			//    int x, y, i;
			//    DrawParams drawParams;
			//    for (y = 0; y < getHeight(); y++) {
			//        for (x = 0; x < getWidth(); x++) {
			//            i = (y * getWidth()) + x;
			//            drawParams = GetDrawParamsByMode(drawMode, i);
			//            spriteBatch.Draw(drawParams.texture, new Vector2(x * 32, y * 32), drawParams.color);
			//        }
			//    }
			//}

			//private DrawParams GetDrawParamsByMode(DRAW_MODE drawMode, int tileIndex) {
			//    DrawParams dParams = new DrawParams();
			//    dParams.texture = tileTexture;
			//    dParams.color = Color.White;

			//    if (drawMode == DRAW_MODE.Shape) {
			//        dParams.color = data.pointData[tileIndex].land ? Color.White : Color.Black;
			//    }
			//    else if (drawMode == DRAW_MODE.Elevation) {
			//        dParams.color = new Color(new Vector3(1, 1, 1) * data.pointData[tileIndex].elevation);
			//    }
			//    else if (drawMode == DRAW_MODE.Temperature) {
			//        dParams.color = new Color(new Vector3(1, 1, 1) * data.pointData[tileIndex].temperature);
			//    }
			//    else if (drawMode == DRAW_MODE.Moisture) {
			//        dParams.color = new Color(new Vector3(1, 1, 1) * data.pointData[tileIndex].moisture);
			//    }
			//    else if (drawMode == DRAW_MODE.Biome) {
			//        if (data.pointData[tileIndex].land) {
			//            dParams.color = biomeColorMap[data.pointData[tileIndex].biome];
			//        }
			//        else {
			//            dParams.color = Color.DarkBlue;
			//        }
			//    }
			//    else if (drawMode == DRAW_MODE.Debug1) {
			//        dParams.color = new Color(new Vector3(1, 1, 1) * data.pointData[tileIndex].debugValue1);
			//    }
			//    else if (drawMode == DRAW_MODE.Debug2) {
			//        dParams.color = new Color(new Vector3(1, 1, 1) * data.pointData[tileIndex].debugValue2);
			//    }

			//    return dParams;
			//}
		}
}
