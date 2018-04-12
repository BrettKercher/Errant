using System;
using ContentExtensionLibrary;
using Errant.src.Loaders;
using Errant.src.Scenes;

namespace Errant.src.GameObjects {
    public class ItemStack : Entity {
        
        public int ItemId { get; set; }
        public int ItemCount { get; set; }

        private DateTime lastUsedTime = DateTime.MinValue;
        
        public ItemStack(Application application) : base(application) {
        }

        public void Use(bool firstClick, int targetTileX, int targetTileY) {
            ItemDefinition itemDef = ItemManager.GetItemDefinitionById(ItemId);

            if (itemDef == null) {
                System.Diagnostics.Debug.WriteLine("No Item definition found for ID: " + ItemId);
                return;
            }

            if (!itemDef.AutoUsable && !firstClick) {
                return;
            }

            if (DateTime.Now.CompareTo(lastUsedTime.AddSeconds(itemDef.UseCooldown)) < 0) {
                return;
            }
            
            lastUsedTime = DateTime.Now;

            if (itemDef.Tool) {
                // tool, harvest object at tile location
                application.GetCurrentWorldManager().RemoveObjectAt(targetTileX, targetTileY);
            }

            if (itemDef.PlacedObject > 0) {
                application.GetCurrentWorldManager().PlaceObjectAt(itemDef.PlacedObject, targetTileX, targetTileY);
            }

            if (itemDef.Consumable) {
                ItemCount--;
                if (ItemCount == 0) {
                    //delete
                }
            }
        }
    }
}
