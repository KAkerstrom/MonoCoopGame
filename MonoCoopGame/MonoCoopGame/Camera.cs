using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame
{
    public class Camera
    {
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public Point Center { get; private set; }
        public Viewport View { get; private set; }
        public Matrix Transform { get; private set; }
        //public Point MoveDestination { get; private set; }
        //public float MoveSpeed { get; private set; }

        public Camera (Viewport view, int x = 0, int y = 0, float zoom = 2f, float rotation = 0)
        {
            View = view;
            Center = new Point(x, y);
            Zoom = zoom;
            Rotation = rotation;
        }

        public void UpdateTransform()
        {
            Matrix center = Matrix.CreateTranslation(-Center.X, -Center.Y, 0);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1.0f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix offset = Matrix.CreateTranslation(View.Width / 2, View.Height / 2, 0);
            Transform = center * scale * rotation * offset;
        }

        public void SetCenter(int x, int y)
        {
            Center = new Point(x, y);
            UpdateTransform();
        }

        public void SetZoom(float zoom)
        {
            Zoom = zoom;
            UpdateTransform();
        }

        public void SetView(Viewport view)
        {
            View = view;
            UpdateTransform();
        }
    }
}