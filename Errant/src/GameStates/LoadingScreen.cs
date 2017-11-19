using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace Errant.src.GameStates {
    class LoadingScreen : IGameState {

        private GameContext context = null;
        private WorldManager map = null;
        private BackgroundWorker mapWorker = null;
        private int progress = 0;

        public LoadingScreen(GameContext _context) {

            context = _context;

            // Initialize a BackgroundWorker to build the map
            map = new WorldManager();
            mapWorker = new BackgroundWorker();
            mapWorker.WorkerReportsProgress = true;
            mapWorker.WorkerSupportsCancellation = true;
            mapWorker.DoWork += OnLoadMapDoWorkHandler;
            mapWorker.ProgressChanged += OnLoadMapProgressChangedHandler;
            mapWorker.RunWorkerCompleted += OnLoadMapCompleteHandler;
            mapWorker.RunWorkerAsync();
        }

        public void Initialize(ContentManager content) {
        }

        public void Dispose(ContentManager content) {
            content.Unload();
        }

        public void Update(GameTime gameTime) {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.DrawString(FontManager.DefaultFont(), progress.ToString() + "%", new Vector2(5, 5), Color.White);
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
            context.SwitchState(new Overworld(map));
        }
    }
}
