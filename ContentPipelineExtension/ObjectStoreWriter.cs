using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = ContentExtensionLibrary.ObjectStore;
using System.Reflection;

namespace ContentPipelineExtension {
    [ContentTypeWriter]
    public class ObjectStoreWriter : ContentTypeWriter<TWrite> {
        protected override void Write(ContentWriter output, TWrite value) {
            output.Write(value.Objects.Count);
            foreach (var obj in value.Objects) {
                output.Write(obj.Id);
                output.Write(obj.Name);
                output.Write(obj.SpriteMapLocation);
                output.Write(obj.SpriteIndex);
                output.Write(obj.Width);
                output.Write(obj.Height);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "ContentExtensionLibrary.ObjectStoreReader, ContentExtensionLibrary";
        }
    }
}