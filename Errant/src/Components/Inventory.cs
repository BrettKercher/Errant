using Errant.src.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Errant.src.Components {
    public class Inventory : Component {

        private ItemStack[] items;
        
        public Inventory(Application application, int size) : base() {
            items = new ItemStack[size];
        }

        /*
         * Adds the given item to the inventory at the given index.
         * Returns the id of the item previously at that index
         */
        public ItemStack AddAndReplace(ItemStack item, uint inventoryIndex) {

            if (inventoryIndex >= items.Length) {
                return null;
            }

            var previousItem = items[inventoryIndex];

            items[inventoryIndex] = item;
            
            return previousItem;
        }

        /*
         * Add the given item to the first available index.
         * Returns whether or not the action was successful
         */
        public bool AddFirstAvailable(ItemStack itemsToAdd) {
            
            for (var i = 0; i < items.Length; i++) {
                if (items[i] == null) {
                    items[i] = itemsToAdd;
                    return true;
                }
                if (items[i].ItemId == itemsToAdd.ItemId) {
                    return true;
                }
            }

            return false;
        }

        public ItemStack GetItemAt(uint inventoryIndex) {
            return inventoryIndex >= items.Length ? null : items[inventoryIndex];
        }

    }
}