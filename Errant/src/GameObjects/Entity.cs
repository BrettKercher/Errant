using Errant.src.Components;
using Errant.src.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Errant.src.GameObjects {
    public class Entity : ICoreComponent {

        //Components
        protected List<ICoreComponent> components;
        protected Transform transform;

        protected Controller controller;

        public Entity(Application application) {
            components = new List<ICoreComponent>();
            transform = new Transform(application);
        }

        public void Dispose() {
            controller = null;
        }

        public void Possess(Controller _controller) {
            controller = _controller;
        }

        public virtual void Initialize(ContentManager content) {
            foreach(ICoreComponent component in components) {
                component.Initialize(content);
            }
        }

        public virtual void Dispose(ContentManager content) {
            foreach (ICoreComponent component in components) {
                component.Dispose(content);
            }
        }

        public virtual void Update(GameTime gameTime) {
            foreach (ICoreComponent component in components) {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (ICoreComponent component in components) {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public virtual void RegisterComponents() {
            components.Add(transform);
        }
    }
}
