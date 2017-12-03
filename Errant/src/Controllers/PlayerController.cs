using Errant.src.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
