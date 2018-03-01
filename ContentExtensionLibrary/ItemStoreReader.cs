using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using TRead = ContentExtensionLibrary.ItemStore;

namespace ContentExtensionLibrary {
    public class ItemStoreReader : ContentTypeReader<TRead> {
        protected override TRead Read(ContentReader input, TRead existingInstance) {

            List<ItemDefinition> items = new List<ItemDefinition>();
            ItemDefinition item;
            int numItems = input.ReadInt32();
            
            for (int i = 0; i < numItems; i++) {
                item = new ItemDefinition();
                item.Id = input.ReadInt32();
                item.Name = input.ReadString();
                item.ToolTip = input.ReadString();
                item.SpriteMapLocation = input.ReadString();
                item.SpriteIndex = input.ReadInt32();
                item.Width = input.ReadInt32();
                item.Height = input.ReadInt32();
                item.MaxStackSize = input.ReadInt32();
                item.UseType = input.ReadInt32();
                item.AutoUsable = input.ReadBoolean();
                item.UseCooldown = input.ReadInt32();
                item.Melee = input.ReadBoolean();
                item.Ranged = input.ReadBoolean();
                item.Magic = input.ReadBoolean();
                item.Tool = input.ReadBoolean();
                item.Consumable = input.ReadBoolean();
                item.PlacedObject = input.ReadUInt16();
                item.PlacedGround = input.ReadUInt16();
                item.Damage = input.ReadInt32();

                items.Add(item);
            }

            var itemStore = new ItemStore {Items = items};

            return itemStore;
        }
    }
}
