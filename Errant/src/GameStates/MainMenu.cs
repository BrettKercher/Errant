using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Errant.src.GameStates {
    class MainMenu : IGameState {

        public void Initialize(ContentManager content) {
        }

        public void Dispose(ContentManager content) {

        }

        public void Update(GameTime gameTime) {
            Debug.WriteLine("Update from Main Menu");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        }
    }
}
