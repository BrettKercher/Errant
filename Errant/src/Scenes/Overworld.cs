using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Errant.src.World;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.Scenes {
    class Overworld : Scene {
        
        private WorldManager worldManager = null;
        private int prevScrollValue = 0;
		private static readonly int CHUNK_LOAD_RADIUS = 1;

        public Overworld(Application _application, WorldManager manager) : base(_application) {
            worldManager = manager;
        }

        public override void Initialize(ContentManager content) {
            worldManager.LoadContent(content);
        }

        public override void Dispose(ContentManager content) {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			worldManager.DrawWorld(gameTime, spriteBatch, 1, CHUNK_LOAD_RADIUS);
        }
    }
}
