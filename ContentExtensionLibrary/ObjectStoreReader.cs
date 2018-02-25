using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using TRead = ContentExtensionLibrary.ObjectStore;

namespace ContentExtensionLibrary {
    class ObjectStoreReader : ContentTypeReader<TRead> {
        protected override TRead Read(ContentReader input, TRead existingInstance) {

            List<ObjectDefinition> objs = new List<ObjectDefinition>();
            ObjectDefinition obj;
            int numObjects = input.ReadInt32();
            
            for (int i = 0; i < numObjects; i++) {
                obj = new ObjectDefinition();
                obj.Id = input.ReadInt32();
                obj.Name = input.ReadString();
                obj.SpriteMapLocation = input.ReadString();
                obj.SpriteIndex = input.ReadInt32();
                obj.Width = input.ReadInt32();
                obj.Height = input.ReadInt32();
                objs.Add(obj);
            }

            var objStore = new ObjectStore {Objects = objs};

            return objStore;
        }
    }
}
