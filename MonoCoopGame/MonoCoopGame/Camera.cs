using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monoCoopGame
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public float Zoom { get; private set; }
        public Point Center { get; private set; }
        public float Rotation { get; private set; }
        //public Point MoveDestination { get; private set; }
        //public float MoveSpeed { get; private set; }

        private Viewport view;

        public Camera (Viewport view, int x = 0, int y = 0, float zoom = 3f, float rotation = 0)
        {
            this.view = view;
            Center = new Point(x - (view.Width / 2), y - (view.Height / 2));
            //Center = new Point(x, y);
            Zoom = zoom;
            Rotation = rotation;
        }

        private void UpdateTransform()
        {
            Matrix center = Matrix.CreateTranslation(-Center.X, -Center.Y, 0);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1.0f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix offset = Matrix.CreateTranslation(view.Width / 2, view.Height / 2, 0);
            Transform = center * scale * rotation * offset;
        }

        public void SetCenter(int x, int y)
        {
            //Center = new Point(x - (int)(view.Width / 2), y - (int)(view.Height / 2));
            Center = new Point(x, y);
            UpdateTransform();
        }

        public void SetZoom(float zoom)
        {
            Zoom = zoom;
            UpdateTransform();
        }
    }
}