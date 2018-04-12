using System.ComponentModel;
using Errant.src.Loaders;
using Errant.src.Networking;
using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Errant.src.Scenes {
    public class LoadingScreen : Scene {
        
        private int progress;
        private WorldManager map;
        private string worldName;
        private bool localWorld;

        public LoadingScreen(Application _application, string _worldName, bool _localWorld): base(_application) {
            worldName = _worldName;
            localWorld = _localWorld;
            map = new WorldManager();
        }
        
        public override void Initialize(ContentManager content) {
            
            if (localWorld) {
                // either non-networked, or you're the host
                BackgroundWorker mapWorker = new BackgroundWorker();
                mapWorker.WorkerReportsProgress = true;
                mapWorker.WorkerSupportsCancellation = false;
                mapWorker.DoWork += OnLoadMapDoWorkHandler;
                mapWorker.ProgressChanged += OnLoadMapProgressChangedHandler;
                mapWorker.RunWorkerCompleted += OnLoadMapCompleteHandler;
                mapWorker.RunWorkerAsync();
            }
            else {
                // if we're the client, we just need to wait for the server to send us the world header
                Client.WorldDetailsReceived += OnWorldHeaderReceived;
            }
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.DrawString(FontManager.DefaultFont(), progress + "%", new Vector2(5, 5), Color.White);
        }

        private void OnLoadMapDoWorkHandler(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            map.LoadWorld(worldName);
        }

        private void OnLoadMapProgressChangedHandler(object sender, ProgressChangedEventArgs e) {
            progress = e.ProgressPercentage;
        }

        private void OnLoadMapCompleteHandler(object sender, RunWorkerCompletedEventArgs e) {
            application.SwitchScene(new Overworld(application, map));
//            application.SwitchScene(new WorldViewer(application, map));
        }

        private void OnWorldHeaderReceived(WorldHeader header) {
            map.GenerateEmptyWorld(header);
            OnLoadMapCompleteHandler(null, null);
        }
    }
}