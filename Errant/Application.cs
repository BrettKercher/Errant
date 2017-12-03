using Errant.src;
using Errant.src.Controllers;
using Errant.src.Scenes;
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
			Content.RootDirectory = "Content";
        }

		protected override void Initialize() {
			IsMouseVisible = true;

            playerController = new PlayerController(this);
            Components.Add(playerController);

            // SwitchState(new GenerationScreen(this));
            SwitchScene(new MainMenu(this));
            Camera2D.Init(GraphicsDevice.Viewport);
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

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera2D.Instance.TransformMatrix);

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


//Scene has all of the GameObjects in it

//Scene receives Update/Draw/etc[0] calls from Application

//Scene calls corresponding method for each GameObject, which calls on components

//[0] Constructor, Initialize, Dispose, Update, Draw