using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentExtensionLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Errant.src.Components;
using Errant.src.Loaders;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Errant.src.GameObjects {
    class Player : Entity {
        
        private readonly SpriteRenderer renderer;
        private readonly Inventory inventory;
        private readonly Inventory hotBar;
        private uint activeIndex;

        public Player(Application application) : base(application) {
            renderer = new SpriteRenderer(application, transform);
            inventory = new Inventory(application, 10);
            hotBar = new Inventory(application, 10);

            hotBar.AddFirstAvailable(new ItemStack(application) {
                ItemCount = 1,
                ItemId = 1
            });
            hotBar.AddFirstAvailable(new ItemStack(application) {
                ItemCount = 999,
                ItemId = 2
            });
            hotBar.AddFirstAvailable(new ItemStack(application) {
                ItemCount = 999,
                ItemId = 3
            });
        }

        public override void RegisterComponents() {
            base.RegisterComponents();
            components.Add(renderer);
            components.Add(inventory);
            components.Add(hotBar);
        }

        public override void Initialize(ContentManager content) {
            base.Initialize(content);
            Camera2D.Instance.SetTarget(transform);
            renderer.SetPivot(SpriteRenderer.Pivot.CENTER);

            activeIndex = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D1)) {
                activeIndex = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.D2)) {
                activeIndex = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.D3)) {
                activeIndex = 2;
            }
            
            base.Update(gameTime);
        }

        public void UseActiveItem(bool firstClick, int targetTileX, int targetTileY) {
            ItemStack activeItem = hotBar.GetItemAt(activeIndex);
            activeItem?.Use(firstClick, targetTileX, targetTileY);
        }
    }
}
