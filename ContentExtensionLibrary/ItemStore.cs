using System.Collections.Generic;

namespace ContentExtensionLibrary {
    public class ItemStore {
        public IList<ItemDefinition> Items { get; set; }
    }

    public class ItemDefinition {
        
        // Unique Identifier
        public int Id { get; set; }
        
        // Localization Token referring to item name
        public string Name { get; set; }
        
        // Localization Token reffering to item tooltip
        public string ToolTip { get; set; }
        
        // Location of the containing sprite map
        public string SpriteMapLocation { get; set; }
        
        //Index in the containing sprite map from the top-left corner
        public int SpriteIndex { get; set; }
        
        // Width of the item sprite, in indices
        public int Width { get; set; }
        
        // Height of the item sprite, in indices
        public int Height { get; set; }
        
        // Maximum number of items that can be in one stack
        public int MaxStackSize { get; set; }
        
        // Category of behavior when the item is used. EG: A potion is consumed, while a sword is swung)
        // 0 = Nothing Happens
        // 1 = Harvest Object
        public int UseType { get; set; }
        
        // Can the item be used repeatedly with a press-and-hold of the use button
        public bool AutoUsable { get; set; }
        
        // Time needed between uses
        public int UseCooldown { get; set; }
        
        // Item Deals Melee Damage
        public bool Melee { get; set; }
        
        // Item Deals Ranged Damage
        public bool Ranged { get; set; }
        
        // Item Deals Magic Damage
        public bool Magic { get; set; }
        
        // Item is capable of harvesting Objects
        public bool Tool { get; set; }
        
        // Wether or not the item should be consumed on use
        public bool Consumable { get; set; }
        
        // Item that should be placed when used
        public ushort PlacedObject { get; set; }
        
        // Ground that should be placed when used
        public ushort PlacedGround { get; set; }
        
        public int Damage { get; set; }
        
    }
    
}
