using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Errant.src {
    static class FontManager {

        private static bool loaded;
        private static SpriteFont defaultFont;

        public static void LoadFonts(ContentManager content) {
            defaultFont = content.Load<SpriteFont>("fonts/default");
            loaded = true;
        }

        public static SpriteFont DefaultFont() {
            Debug.Assert(loaded);
            return defaultFont;
        }
    }
}
