using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json.Linq;

using TInput = Newtonsoft.Json.Linq.JObject;


namespace ContentPipelineExtension {
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>

    [ContentImporter(".itemStore", DisplayName = "ItemStore Importer", DefaultProcessor = "ItemStoreProcessor")]
    public class ItemStoreImporter : ContentImporter<TInput> {

        public override TInput Import(string filename, ContentImporterContext context) {
            return JObject.Parse(File.ReadAllText(filename));
        }

    }

}
