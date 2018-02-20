using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Errant.src.Components {
    public class Component : ICoreComponent {
        public virtual void Dispose(ContentManager content) {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        }

        public virtual void Initialize(ContentManager content) {
        }

        public virtual void Update(GameTime gameTime) {
        }
    }
}
