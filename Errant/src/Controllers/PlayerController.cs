using Errant.src.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.Controllers {
    public class PlayerController : Controller {

        public PlayerController(Application _application) : base(_application) {

        }

        public override void Possess(Entity entity) {
            possessedEntity = entity;
            entity.Possess(this);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            Player playerEntity = (Player) possessedEntity;

            if (playerEntity == null) {
                return;
            }

            ProcessMovement();

            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) {
                Vector2 tilePos = Camera2D.Instance.ScreenToWorldSpace(new Vector2(mouseState.X, mouseState.Y));
                int tileX = (int)(tilePos.X / Config.TILE_SIZE);
                int tileY = (int)(tilePos.Y / Config.TILE_SIZE);
            }
        }

        private void ProcessMovement() {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 movementVector = new Vector2(0, 0);
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) {
                movementVector.Y -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
                movementVector.X -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) {
                movementVector.Y += 1;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) {
                movementVector.X += 1;
            }

            if (!movementVector.Equals(Vector2.Zero)) {
                possessedEntity.Move(movementVector);
            }
        }
    }
}
