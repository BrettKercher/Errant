using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Errant.src.World;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.GameStates {
    class Overworld : IGameState {
        
        private WorldManager worldManager = null;
        private int prevScrollValue = 0;
		private static readonly int CHUNK_LOAD_RADIUS = 1;

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
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			worldManager.DrawWorld(gameTime, spriteBatch, 1, CHUNK_LOAD_RADIUS);
        }
    }
}
