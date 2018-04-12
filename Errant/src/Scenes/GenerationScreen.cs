using Errant.src.Loaders;
using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace Errant.src.Scenes {
    public class GenerationScreen : Scene {
        
        private int progress;
        private GenerationSettings generationSettings;

        public GenerationScreen(Application _application, GenerationSettings genSettings) : base(_application) {
            application = _application;
            generationSettings = genSettings;
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
            
            WorldManager map = new WorldManager();
            map.GenerateWorld(generationSettings, worker);
            map.SaveWorld();
        }

        private void OnLoadMapProgressChangedHandler(object sender, ProgressChangedEventArgs e) {
            progress = e.ProgressPercentage;
        }

        private void OnLoadMapCompleteHandler(object sender, RunWorkerCompletedEventArgs e) {
            application.SwitchScene(new MainMenu(application, true));
        }
    }
}
