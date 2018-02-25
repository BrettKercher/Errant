using ContentExtensionLibrary;
using Errant.src.Loaders;

namespace Errant.src.GameObjects {
    public class ItemStack {
        
        public int ItemId { get; set; }
        public int ItemCount { get; set; }

        public void Use(bool firstClick, int targetTileX, int targetTileY) {
            ItemDefinition itemDef = ItemManager.GetItemDefinitionById(ItemId);

            if (itemDef == null) {
                System.Diagnostics.Debug.WriteLine("No Item definition found for ID: " + ItemId);
                return;
            }

            if (!itemDef.AutoUsable && !firstClick) {
                return;
            }
        }
    }
}


// item types:
// tool - use = activate (pickaxe swing, staff cast)
// consumable - use = consume (potions, food)
// placeable object - use = place (tiles, chests, furnaces)
// placeable wall
// placeable floor
// equipment - use = nothing (armor, accessory)
