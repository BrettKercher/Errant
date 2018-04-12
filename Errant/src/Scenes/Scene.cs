using Errant.src.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Errant.src.World;

namespace Errant.src.Scenes {
    public class Scene : ICoreComponent {

        protected Application application;
        protected List<Entity> entities;

        public Scene(Application _application) {
            application = _application;
            entities = new List<Entity>();
        }

        public virtual void Dispose(ContentManager content) {
            foreach(Entity entity in entities) {
                entity.Dispose(content);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (Entity entity in entities) {
                entity.Draw(gameTime, spriteBatch);
            }
        }

        public virtual void Initialize(ContentManager content) {
            foreach (Entity entity in entities) {
                entity.Initialize(content);
            }
        }

        public virtual void Update(GameTime gameTime) {
            foreach (Entity entity in entities) {
                entity.Update(gameTime);
            }
        }

        public void AddEntity(Entity entity) {
            entity.RegisterComponents();
            entities.Add(entity);
        }

        public virtual WorldManager GetWorldManager() {
            return null;
        }
    }
}
