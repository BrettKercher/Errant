﻿using Errant.src.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Errant.src {
    class Camera2D : GameComponent{

        private static Camera2D instance;

        private float zoom = 1.0f;
        private Vector2 position = new Vector2(0, 0);
        private float rotation = 0.0f;

        private Rectangle Bounds { get; set; }

        private Transform target;

        private Camera2D(Application application, Viewport viewport) : base(application) {
            Bounds = viewport.Bounds;
        }

        public static Camera2D Instance {
            get { return instance; }
        }

        public static void Init(Application application, Viewport viewPort) {
            instance = new Camera2D(application, viewPort);
        }

        public Matrix TransformMatrix {
            get {
                return
                    Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        public void MoveTo(Vector2 newPosition) {
            position = newPosition;
        }

        public void Zoom(float zoomAmount) {
            zoom += zoomAmount;
            zoom = Math.Max(0.01f, zoom);

        }

        public void Pan(Vector2 moveAmount) {
            position += moveAmount;
        }

        public void Rotate(float rotateAmount) {
            rotation += rotateAmount;
        }

        public Vector2 ScreenToWorldSpace(Vector2 screenPos) {
            return Vector2.Transform(screenPos, Matrix.Invert(TransformMatrix));
        }

        public Vector2 WorldToScreenSpace(Vector2 worldPos) {
            return Vector2.Transform(worldPos, TransformMatrix);
        }

        public void SetTarget(Transform targetTransform) {
            target = targetTransform;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (target == null) {
                return;
            }

            MouseState mouseState = Mouse.GetState();
            Vector2 goalPos;
            Vector2 mousePos = ScreenToWorldSpace(new Vector2(mouseState.Position.X, mouseState.Position.Y));
            goalPos = ((7 * target.Position) + mousePos) / 8;

            position = Vector2.Lerp(position, goalPos, 0.075f);

            position.X = (float)Math.Round(position.X, 1);
            position.Y = (float)Math.Round(position.Y, 1);

        }
    }
}
