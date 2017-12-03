using Errant.src.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Errant.src.Components {
    public class Transform : Component {

        Vector2 position;
        Vector2 scale;
        float rotation;

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        public float Rotation {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale {
            get { return scale; }
            set { scale = value; }
        }

        public Transform(Game game) {
            position = Vector2.Zero;
            scale = new Vector2(1, 1);
            rotation = 0;
        }

        public override void Dispose(ContentManager content) {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        }

        public override void Initialize(ContentManager content) {
        }

        public override void Update(GameTime gameTime) {
            rotation += 0.1f;
        }
    }
}
