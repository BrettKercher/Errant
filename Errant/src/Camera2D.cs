using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src {
    class Camera2D {

        private static Camera2D instance;

        private float zoom = 1.0f;
        private Vector2 position = new Vector2(0, 0);
        private float rotation = 0.0f;

        private Rectangle Bounds { get; set; }

        private Camera2D(Viewport viewport) {
            Bounds = viewport.Bounds;
        }

        public static Camera2D Instance {
            get { return instance; }
        }

        public static void Init(Viewport viewPort) {
            instance = new Camera2D(viewPort);
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

        public void PanTo(Vector2 newPosition) {

        }

        public void Zoom(float zoomAmount) {
            zoom += zoomAmount;
            zoom = Math.Max(0.01f, zoom);

        }

        public void Move(Vector2 moveAmount) {
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
    }
}
