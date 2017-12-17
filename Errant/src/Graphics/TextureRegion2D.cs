using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.Graphics {
    class TextureRegion2D {

        public TextureRegion2D(Texture2D texture, int x, int y, int width, int height ) {
            Texture = texture;
            SourceRectangle = new Rectangle(x, y, width, height);
        }

        public Texture2D Texture { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
    }
}
