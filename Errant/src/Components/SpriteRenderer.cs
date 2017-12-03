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

        public enum Pivot {
            TOP_LEFT,
            TOP,
            TOP_RIGHT,
            LEFT,
            CENTER,
            RIGHT,
            BOTTOM_LEFT,
            BOTTOM,
            BOTTOM_RIGHT
        }

        // Multiply x by width and y by height to get correct origin value
        private Dictionary<Pivot, Vector2> pivotOriginMap = new Dictionary<Pivot, Vector2>() {
            { Pivot.TOP_LEFT,       new Vector2(0.0f,   0) },
            { Pivot.TOP,            new Vector2(0.5f,   0) },
            { Pivot.TOP_RIGHT,      new Vector2(1.0f,   0) },
            { Pivot.LEFT,           new Vector2(0.0f,   0.5f) },
            { Pivot.CENTER,         new Vector2(0.5f,   0.5f) },
            { Pivot.RIGHT,          new Vector2(1.0f,   0.5f) },
            { Pivot.BOTTOM_LEFT,    new Vector2(0.0f,   1.0f) },
            { Pivot.BOTTOM,         new Vector2(0.5f,   1.0f) },
            { Pivot.BOTTOM_RIGHT,   new Vector2(1.0f,   1.0f) },
        };

        Texture2D texture;
        Transform transform;
        Vector2 origin;

        public Vector2 Origin {
            get { return origin; }
            set { origin = value; }
        }

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
            spriteBatch.Draw(texture, transform.Position, null, Color.White, transform.Rotation, Origin, transform.Scale, SpriteEffects.None, 0);
        }        

        public void SetPivot(Pivot pivot) {
            origin = new Vector2(
                pivotOriginMap[pivot].X * texture.Width, 
                pivotOriginMap[pivot].Y * texture.Height
            );
        }
    }
}
