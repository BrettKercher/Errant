using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = ContentExtensionLibrary.ItemStore;

namespace ContentPipelineExtension {
    [ContentTypeWriter]
    public class ItemStoreWriter : ContentTypeWriter<TWrite> {
        protected override void Write(ContentWriter output, TWrite value) {
            output.Write(value.Items.Count);
            foreach (var item in value.Items) {
                output.Write(item.Id);
                output.Write(item.Name);
                output.Write(item.ToolTip);
                output.Write(item.SpriteMapLocation);
                output.Write(item.SpriteIndex);
                output.Write(item.Width);
                output.Write(item.Height);
                output.Write(item.MaxStackSize);
                output.Write(item.UseType);
                output.Write(item.AutoUsable);
                output.Write(item.UseCooldown);
                output.Write(item.Melee);
                output.Write(item.Ranged);
                output.Write(item.Magic);
                output.Write(item.Tool);
                output.Write(item.Consumable);
                output.Write(item.PlacedObject);
                output.Write(item.PlacedGround);
                output.Write(item.Damage);

            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "ContentExtensionLibrary.ItemStoreReader, ContentExtensionLibrary";
        }
    }
}
