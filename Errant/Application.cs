using Errant.src;
using Errant.src.Controllers;
using Errant.src.Scenes;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Errant {

	public class Application : Game {
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private Scene currentScene;
        private GameMode gameMode;
        private PlayerController playerController;

        public Application() {
			graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

		protected override void Initialize() {
			IsMouseVisible = true;

            playerController = new PlayerController(this);
            
            Camera2D.Init(this, GraphicsDevice.Viewport);
            SwitchScene(new GenerationScreen(this));

            Components.Add(playerController);
            Components.Add(Camera2D.Instance);

            base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			FontManager.LoadFonts(Content);
		}

		protected override void UnloadContent() {
			currentScene.Dispose(Content);
			Content.Unload();
		}

		protected override void Update(GameTime gameTime) {
			currentScene.Update(gameTime);

            base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                BlendState.AlphaBlend, 
                SamplerState.PointClamp, 
                null, 
                null, 
                null, 
                Camera2D.Instance.TransformMatrix
            );

			currentScene.Draw(gameTime, spriteBatch);

			spriteBatch.End();

            base.Draw(gameTime);
		}

		public void SwitchScene(Scene newScene) {
			if (newScene == null) {
				return;
			}

			if (currentScene != null) {
				currentScene.Dispose(Content);
			}

            newScene.Initialize(Content);
			currentScene = newScene;
		}

        public PlayerController GetPlayerController() {
            return playerController;
        }
	}
}
