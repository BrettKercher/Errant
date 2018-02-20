using ContentExtensionLibrary;
using Errant.src.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.Loaders {
    static class ObjectManager {
        
        private static ObjectStore objectStore;
        private static Dictionary<int, ObjectDefinition> objectMap;
        private static Dictionary<string, TextureAtlas> objectTextureMaps;

        static Texture2D texture;

        public static void LoadObjects(ContentManager content) {
            objectMap = new Dictionary<int, ObjectDefinition>();
            objectTextureMaps = new Dictionary<string, TextureAtlas>();
            objectStore = content.Load<ObjectStore>("data/objects_01");

            foreach (var obj in objectStore.Objects) {
                if(!objectTextureMaps.ContainsKey(obj.SpriteMapLocation)) {
                    Texture2D spriteMap = content.Load<Texture2D>(obj.SpriteMapLocation);
                    TextureAtlas t = new TextureAtlas(spriteMap, 8, 4, Config.TILE_SIZE, Config.TILE_SIZE);
                    objectTextureMaps.Add(obj.SpriteMapLocation, t);
                }
                objectMap.Add(obj.Id, obj);
            }
        }

        public static ObjectDefinition GetObjectDefinitionById(int id) {
            return objectMap[id];
        }

        public static TextureRegion2D GetObjectTextureRegionById(int id) {
            ObjectDefinition objDef = objectMap[id];
            TextureAtlas atlas = objectTextureMaps[objDef.SpriteMapLocation];
            return atlas.GetTextureRegion(objDef.SpriteIndex);
        }
    }
}
