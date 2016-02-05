using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class CameraManager
    {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public Rectangle Bounds { get; set; }

        public Matrix TransformMatrix
        {
            get
            {
                Matrix m = Matrix.CreateTranslation(new Vector3((Position.X + 16) * -1, (Position.Y + 16) * -1, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));

                int mapWidth = MapManager.Instance.currentMap.width * 32;
                int mapHeight = MapManager.Instance.currentMap.height * 32;

                int widthComparison = ((int)(mapWidth * Zoom) - Bounds.Width) * -1;
                int heightComparison = ((int)(mapHeight * Zoom) - Bounds.Height) * -1;

                if (widthComparison > 0)
                    widthComparison = 0;
                if (heightComparison > 0)
                    heightComparison = 0;

                if (m.Translation.X > 0)
                {
                    m *= Matrix.CreateTranslation(new Vector3(-m.Translation.X, 0, 0));
                } else if (m.Translation.X < widthComparison) {
                    m *= Matrix.CreateTranslation(new Vector3(widthComparison - m.Translation.X, 0, 0));
                }

                if (m.Translation.Y > 0)
                {
                    m *= Matrix.CreateTranslation(new Vector3(0, -m.Translation.Y, 0));
                } else if (m.Translation.Y < heightComparison)
                {
                    m *= Matrix.CreateTranslation(new Vector3(0, heightComparison - m.Translation.Y, 0));
                }

                //Console.WriteLine(String.Format("Translation: {0} - WidthComp: {1} - HeightComp: {2}", m.Translation, widthComparison, heightComparison));

                return m;
            }
        }

        // Singleton stuff
        private static CameraManager instance;
        protected CameraManager()
        {
            Zoom = 1;
            Rotation = 0;
        }
        public static CameraManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CameraManager();
                }
                return instance;
            }
        }
    }
}
