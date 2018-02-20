using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ContentExtensionLibrary {
    public class ObjectStore {
        public IList<ObjectDefinition> Objects { get; set; }
    }

    public class ObjectDefinition {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpriteMapLocation { get; set; }
        public int SpriteIndex { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
