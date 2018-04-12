using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Errant.src.World;
using Microsoft.Xna.Framework.Input;
using Errant.src.GameObjects;
using Errant.src.Controllers;
using Errant.src.Components;

namespace Errant.src.Scenes {
    class Overworld : Scene {
        
        private WorldManager worldManager;
		private static readonly int CHUNK_LOAD_RADIUS = 1;

        private MouseState prevMouseState;

        private Player player;
        private Transform playerTransform;
        private PlayerController playerController;

        public Overworld(Application _application, WorldManager manager) : base(_application) {
            worldManager = manager;

            player = new Player(_application);
            AddEntity(player);

            playerController = application.GetPlayerController();
            playerController.Possess(player);

            prevMouseState = Mouse.GetState();
        }

        public override void Initialize(ContentManager content) {
            base.Initialize(content);
            worldManager.LoadContent(content);
            playerTransform = (Transform)player.GetComponent(typeof(Transform));
            playerTransform.Position = new Vector2(208 * Config.TILE_SIZE, 144 * Config.TILE_SIZE);
            Camera2D.Instance.MoveTo(playerTransform.Position);
        }

        public override void Dispose(ContentManager content) {
            base.Dispose(content);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            MouseState mouseState = Mouse.GetState();

            if(mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed) {

                Vector2 tilePos = Camera2D.Instance.ScreenToWorldSpace(new Vector2(mouseState.X, mouseState.Y));
                int tileX = (int)(tilePos.X / Config.TILE_SIZE);
                int tileY = (int)(tilePos.Y / Config.TILE_SIZE);

                // worldManager.PrintTileData(tileX, tileY);
            }
            
            prevMouseState = mouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			worldManager.DrawWorld(gameTime, spriteBatch, 
                worldManager.GetContainingChunkIndex(playerTransform.Position), CHUNK_LOAD_RADIUS);
            base.Draw(gameTime, spriteBatch);
        }

        public override WorldManager GetWorldManager() {
            return worldManager;
        }
    }
}
