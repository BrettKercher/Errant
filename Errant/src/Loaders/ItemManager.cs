using ContentExtensionLibrary;
using Errant.src.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Errant.src.Loaders {
    static class ItemManager {
        
        private static ItemStore itemStore;
        private static Dictionary<int, ItemDefinition> itemMap;
        private static Dictionary<string, TextureAtlas> itemTextureMaps;

        public static void LoadItems(ContentManager content) {
            itemMap = new Dictionary<int, ItemDefinition>();
            itemTextureMaps = new Dictionary<string, TextureAtlas>();
            itemStore = content.Load<ItemStore>("data/items_01");

            foreach (var item in itemStore.Items) {
                if(!itemTextureMaps.ContainsKey(item.SpriteMapLocation)) {
                    Texture2D spriteMap = content.Load<Texture2D>(item.SpriteMapLocation);
                    TextureAtlas t = new TextureAtlas(spriteMap, 8, 4, Config.TILE_SIZE, Config.TILE_SIZE);
                    itemTextureMaps.Add(item.SpriteMapLocation, t);
                }
                itemMap.Add(item.Id, item);
            }
        }

        public static ItemDefinition GetItemDefinitionById(int id) {
            return itemMap[id];
        }

        public static TextureRegion2D GetItemTextureRegionById(int id) {
            ItemDefinition itemDef = itemMap[id];
            TextureAtlas atlas = itemTextureMaps[itemDef.SpriteMapLocation];
            return atlas.GetTextureRegion(itemDef.SpriteIndex);
        }
    }
}
