using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Maps
{
    public class MapObject
    {
        // Texture stuff
        public Texture2D texture { get; set; }
        public int textureId { get; set; }

        // Positioning stuff (tile-based)
        public Vector2 mapPosition { get; set; }

        // Moving stuff (pixel-based)
        public bool isMoving { get; set; }
        
        public Vector2 currentPosition { get; set; }
        public Vector2 destinationPosition { get; set; }

        double lerpTime = 0;
        double animationSpeed = 5;

        public MapObject() { }

        public MapObject(TmxObject obj)
        {
            texture = Global.Content.Load<Texture2D>("charsets"); //TODO
            textureId = obj.Tile.Gid - 1;
            mapPosition = new Vector2((int)obj.X / Global.tileSize, (int)obj.Y / Global.tileSize);
        }

        public void Update(double timePassed)
        {
            int size = Global.tileSize;

            currentPosition = mapPosition * size;

            // Calculate moving animation
            if (isMoving)
            {
                int x = (int)(currentPosition.X + (destinationPosition.X - currentPosition.X) * lerpTime);
                int y = (int)(currentPosition.Y + (destinationPosition.Y - currentPosition.Y) * lerpTime);
                currentPosition = new Vector2(x, y);
                lerpTime += timePassed * animationSpeed;

                // Finished movement
                if (lerpTime >= 1)
                {
                    lerpTime = 0;
                    isMoving = false;
                    mapPosition = destinationPosition / size;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            int size = Global.tileSize;

            int column = textureId % (texture.Height / Global.tileSize);
            int row = textureId / (texture.Width / Global.tileSize);
            
            Rectangle tilesetRec = new Rectangle(size * column, size * row, size, size);
            spriteBatch.Draw(texture, new Rectangle((int)currentPosition.X, (int)currentPosition.Y, size, size), tilesetRec, Color.White);
        }
    }
}
