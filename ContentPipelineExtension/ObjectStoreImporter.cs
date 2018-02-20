using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
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

    [ContentImporter(".objectStore", DisplayName = "ObjectStore Importer", DefaultProcessor = "ObjectStoreProcessor")]
    public class ObjectStoreImporter : ContentImporter<TInput> {

        public override TInput Import(string filename, ContentImporterContext context) {
            return JObject.Parse(File.ReadAllText(filename));
        }

    }

}
