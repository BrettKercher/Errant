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
        public Transform transform;

        //Movement
        protected Vector2 movementVector;
        protected float moveSpeed = 1.0f;
        private bool wantsToMove = false;

        protected Controller controller;

        public Entity(Application application) {
            components = new List<ICoreComponent>();
            transform = new Transform(application);
        }

        public Component GetComponent(Type componentType) {
            foreach (Component component in components) {
                if (component.GetType() == componentType) {
                    return component;
                }
            }
            return null;
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

            if(wantsToMove) {
                wantsToMove = false;
                transform.Position += movementVector;
            }

            foreach (ICoreComponent component in components) {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (ICoreComponent component in components) {
                component.Draw(gameTime, spriteBatch);
            }
        }

        // Register components is called automatically when the entity is added to the scene
        public virtual void RegisterComponents() {
            components.Add(transform);
        }

        public void Possess(Controller _controller) {
            controller = _controller;
        }

        public void Move(Vector2 moveVec) {
            wantsToMove = true;
            movementVector = moveVec * moveSpeed;
        }
    }
}
