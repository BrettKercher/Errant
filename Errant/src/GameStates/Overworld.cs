using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Errant.src.World;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.GameStates {
    class Overworld : IGameState {
        
        private WorldManager worldManager = null;
        private int prevScrollValue = 0;
        private WorldManager.DRAW_MODE worldDrawMode = WorldManager.DRAW_MODE.Elevation;

        public Overworld(WorldManager manager) {
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

            if (keyboardState.IsKeyDown(Keys.F1)) {
                worldDrawMode = WorldManager.DRAW_MODE.Shape;
            }
            if (keyboardState.IsKeyDown(Keys.F2)) {
                worldDrawMode = WorldManager.DRAW_MODE.Elevation;
            }
            if (keyboardState.IsKeyDown(Keys.F3)) {
                worldDrawMode = WorldManager.DRAW_MODE.Temperature;
            }
            if (keyboardState.IsKeyDown(Keys.F4)) {
                worldDrawMode = WorldManager.DRAW_MODE.Moisture;
            }
            if (keyboardState.IsKeyDown(Keys.F5)) {
                worldDrawMode = WorldManager.DRAW_MODE.Biome;
            }
            if (keyboardState.IsKeyDown(Keys.F9)) {
                worldDrawMode = WorldManager.DRAW_MODE.Debug1;
            }
            if (keyboardState.IsKeyDown(Keys.F10)) {
                worldDrawMode = WorldManager.DRAW_MODE.Debug2;
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            worldManager.DrawWorld(gameTime, spriteBatch, worldDrawMode);
        }
    }
}
