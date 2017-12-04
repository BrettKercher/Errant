using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.Controllers {
    public class PlayerController : Controller {

        public PlayerController(Application _application) : base(_application) {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (possessedEntity == null) {
                return;
            }

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

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
