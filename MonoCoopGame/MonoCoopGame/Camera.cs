using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoCoopGame
{
    public class Camera
    {
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public Point CurrentPosition { get; private set; }
        public Viewport View { get; private set; }
        public Matrix Transform { get; private set; }
        public Point MoveDestination { get; private set; }
        public int MoveSpeed { get; private set; }

        public Camera(Viewport view, int x = 0, int y = 0, float zoom = 1f, float rotation = 0)
        {
            View = view;
            CurrentPosition = MoveDestination = new Point(x, y);
            MoveSpeed = 4;
            Zoom = zoom;
            Rotation = rotation;
        }

        public void UpdateTransform()
        {
            Matrix center = Matrix.CreateTranslation(-CurrentPosition.X, -CurrentPosition.Y, 0);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1.0f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix offset = Matrix.CreateTranslation(View.Width / 2, View.Height / 2, 0);
            Transform = center * scale * rotation * offset;
        }

        public void SetCenter(int x, int y)
        {
            MoveDestination = new Point(x, y);
        }

        public void UpdateGameCamera(List<Point> playerPositions)
        {
            int xCenter = 0, yCenter = 0;
            foreach (Point p in playerPositions)
            {
                xCenter += p.X;
                yCenter += p.Y;
            }
            xCenter /= playerPositions.Count;
            yCenter /= playerPositions.Count;
            SetCenter(xCenter, yCenter);

            int xDistance = 0, yDistance = 0;
            foreach (Point p in playerPositions)
            {
                if (Math.Abs(p.X - xCenter) > xDistance)
                    xDistance = Math.Abs(p.X - xCenter);
                if (Math.Abs(p.Y - yCenter) > yDistance)
                    yDistance = Math.Abs(p.Y - yCenter);
            }
            float xRatio = xDistance / (View.Width / 2f);
            float yRatio = yDistance / (View.Height / 2f);
            float zoom = 0.3f / MathHelper.Max(xRatio, yRatio);
            SetZoom(Math.Max(0.85f, Math.Min(1.25f, zoom)));
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

        public void MoveTowardDestination()
        {
            int xNew = CurrentPosition.X;
            int yNew = CurrentPosition.Y;
            if (Math.Abs(CurrentPosition.X - MoveDestination.X) < MoveSpeed)
                xNew = MoveDestination.X;
            else if (CurrentPosition.X < MoveDestination.X)
                xNew += MoveSpeed;
            else if (CurrentPosition.X > MoveDestination.X)
                xNew -= MoveSpeed;

            if (Math.Abs(CurrentPosition.Y - MoveDestination.Y) < MoveSpeed)
                yNew = MoveDestination.Y;
            else if (CurrentPosition.Y < MoveDestination.Y)
                yNew += MoveSpeed;
            else if (CurrentPosition.Y > MoveDestination.Y)
                yNew -= MoveSpeed;

            CurrentPosition = new Point(xNew, yNew);
            UpdateTransform();
        }
    }
}