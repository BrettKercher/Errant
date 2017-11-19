using Errant.src;
using Errant.src.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Errant {

    public class GameContext : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private IGameState currentState;

        public GameContext() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            IsMouseVisible = true;
            SwitchState(new LoadingScreen(this));
            Camera2D.Init(GraphicsDevice.Viewport);
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            FontManager.LoadFonts(Content);
        }

        protected override void UnloadContent() {
            currentState.Dispose(Content);
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            currentState.Update(gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera2D.Instance.TransformMatrix);

            currentState.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }

        public void SwitchState(IGameState newState) {
            if(newState == null) {
                return;
            }

            if(currentState != null) {
                currentState.Dispose(Content);
            }

            newState.Initialize(Content);
            currentState = newState;
        }
    }
}
