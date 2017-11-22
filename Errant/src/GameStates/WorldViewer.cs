using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Errant.src.GameStates
{
	class WorldViewer : IGameState {

		private WorldManager worldManager = null;
		private int prevScrollValue = 0;
		private WorldManager.DRAW_MODE worldDrawMode = WorldManager.DRAW_MODE.Biome;

		public WorldViewer(WorldManager manager) {
			worldManager = manager;
		}

		public void Initialize(ContentManager content) {
			worldManager.LoadContent(content);
		}

		public void Dispose(ContentManager content) {

		}

		public void Update(GameTime gameTime) {

			KeyboardState keyboardState = Keyboard.GetState();
			MouseState mouseState = Mouse.GetState();

			if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) {
				Camera2D.Instance.Move(new Vector2(0, -100));
			}

			if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
				Camera2D.Instance.Move(new Vector2(-100, 0));
			}

			if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) {
				Camera2D.Instance.Move(new Vector2(0, 100));
			}

			if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) {
				Camera2D.Instance.Move(new Vector2(100, 0));
			}

			if (prevScrollValue != mouseState.ScrollWheelValue) {
				Camera2D.Instance.Zoom((mouseState.ScrollWheelValue - prevScrollValue) / 12000.0f);
				prevScrollValue = mouseState.ScrollWheelValue;
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			worldManager.DrawWorld(gameTime, spriteBatch, 0, Math.Max(worldManager.GetHeight(), worldManager.GetWidth()));
		}
	}
}
