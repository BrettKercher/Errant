using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.Graphics {
    class TextureAtlas {

        private TextureRegion2D[] textures;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TextureAtlas(Texture2D texture, int width, int height, int textureWidth, int textureHeight) {
            textures = new TextureRegion2D[width * height];

            Width = width;
            Height = height;

            int x = 0;
            int y = 0;

            for(int i = 0; i < textures.Length; i++) {
                x = i % width;
                y = i / width;
                textures[i] = new TextureRegion2D(texture, (x * textureWidth), (y * textureHeight), textureWidth, textureHeight);
            }
        }

        public TextureRegion2D GetTextureRegion(int regionIndex) {
            return textures[regionIndex];
        }
    }
}
