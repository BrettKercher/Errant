using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Errant.src.Components {
    class SpriteRenderer : Component {

        Texture2D texture;
        Transform transform;

        public SpriteRenderer(Application application, Transform _transform) {
            transform = _transform;
        }

        public override void Initialize(ContentManager content) {
            texture = content.Load<Texture2D>("sprites/player");
        }

        public override void Dispose(ContentManager content) {
        }

        public override void Update(GameTime gameTime) {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, transform.Position, null, Color.White, transform.Rotation, Vector2.Zero, transform.Scale, SpriteEffects.None, 0);
        }        
    }
}
