using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Errant.src.Components;
using Microsoft.Xna.Framework.Content;

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

        public override void Initialize(ContentManager content) {
            base.Initialize(content);
            Camera2D.Instance.SetTarget(transform);
            renderer.SetPivot(SpriteRenderer.Pivot.CENTER);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        
    }
}
