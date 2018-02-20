using Errant.src.Controllers;
using Errant.src.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Errant.src.Scenes {
    class MainMenu : Scene {

        protected Entity player;
        protected PlayerController playerController;

        public MainMenu(Application _application) : base(_application) {
            player = new Player(_application);
            AddEntity(player);

            playerController = application.GetPlayerController();
            playerController.Possess(player);
        }

        public override void Initialize(ContentManager content) {
            base.Initialize(content);
        }

        public override void Dispose(ContentManager content) {
            base.Dispose(content);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
