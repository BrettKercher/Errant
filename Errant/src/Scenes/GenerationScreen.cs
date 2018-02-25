using Errant.src.Loaders;
using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace Errant.src.Scenes {
    class GenerationScreen : Scene {
        
        private WorldManager map = null;
        
        private int progress = 0;

        public GenerationScreen(Application _application) : base(_application) {
            application = _application;
            map = new WorldManager();
        }

        public override void Initialize(ContentManager content) {
            // Initialize a BackgroundWorker to build the map
            BackgroundWorker mapWorker = new BackgroundWorker();
            mapWorker.WorkerReportsProgress = true;
            mapWorker.WorkerSupportsCancellation = true;
            mapWorker.DoWork += OnLoadMapDoWorkHandler;
            mapWorker.ProgressChanged += OnLoadMapProgressChangedHandler;
            mapWorker.RunWorkerCompleted += OnLoadMapCompleteHandler;
            mapWorker.RunWorkerAsync();
        }

        public override void Dispose(ContentManager content) {
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.DrawString(FontManager.DefaultFont(), progress + "%", new Vector2(5, 5), Color.White);
        }

        private void OnLoadMapDoWorkHandler(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;

            GenerationSettings genSettings = new GenerationSettings();
            genSettings.size = WorldSize.TINY;
            genSettings.seed = 0;

            map.GenerateWorld(genSettings, worker);
        }

        private void OnLoadMapProgressChangedHandler(object sender, ProgressChangedEventArgs e) {
            progress = e.ProgressPercentage;
        }

        private void OnLoadMapCompleteHandler(object sender, RunWorkerCompletedEventArgs e) {
            application.SwitchScene(new Overworld(application, map));
//            application.SwitchScene(new WorldViewer(application, map));
        }
    }
}
