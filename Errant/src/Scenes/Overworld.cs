﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Errant.src.World;
using Microsoft.Xna.Framework.Input;
using Errant.src.GameObjects;
using Errant.src.Controllers;
using Errant.src.Components;

namespace Errant.src.Scenes {
    class Overworld : Scene {
        
        private WorldManager worldManager = null;
		private static readonly int CHUNK_LOAD_RADIUS = 1;

        protected Player player;
        private Transform playerTransform;
        protected PlayerController playerController;

        public Overworld(Application _application, WorldManager manager) : base(_application) {
            worldManager = manager;

            player = new Player(_application);
            AddEntity(player);

            playerController = application.GetPlayerController();
            playerController.Possess(player);
        }

        public override void Initialize(ContentManager content) {
            base.Initialize(content);
            worldManager.LoadContent(content);
            playerTransform = (Transform)player.GetComponent(typeof(Transform));
        }

        public override void Dispose(ContentManager content) {
            base.Dispose(content);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			worldManager.DrawWorld(gameTime, spriteBatch, 
                worldManager.GetContainingChunkIndex(playerTransform.Position), CHUNK_LOAD_RADIUS);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
