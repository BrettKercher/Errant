using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.Controllers {
    public class PlayerController : Controller {

        public PlayerController(Application _application) : base(_application) {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
        }
    }
}
