using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Errant.src.Components;

namespace Errant.src.GameObjects {
    class Player : Entity {

        SpriteRenderer renderer;

        public Player(Application application) : base(application) {
            renderer = new SpriteRenderer(application, transform);
        }

        public override void RegisterComponents() {
            base.RegisterComponents();
            components.Add(renderer);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}
