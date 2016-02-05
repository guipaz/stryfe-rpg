using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Maps
{
    public class MapObject
    {
        // Texture stuff
        public Texture2D texture { get; set; }
        public int textureId { get; set; }
        protected int tileSize = 32;

        // Positioning stuff
        public int positionX { get; set; }
        public int positionY { get; set; }

        // Moving stuff
        public bool isMoving { get; set; }

        public int tempX { get; set; }
        public int tempY { get; set; }

        public int destinationX { get; set; }
        public int destinationY { get; set; }

        double lerpTime = 0;
        double animationSpeed = 5;

        public void Update(double timePassed)
        {
            tempX = positionX;
            tempY = positionY;

            // Calculate moving animation
            if (isMoving)
            {
                tempX = (int)(positionX + (destinationX - positionX) * lerpTime);
                tempY = (int)(positionY + (destinationY - positionY) * lerpTime);
                lerpTime += timePassed * animationSpeed;
            }

            // Finished movement
            if (lerpTime >= 1)
            {
                lerpTime = 0;
                isMoving = false;
                positionX = destinationX;
                positionY = destinationY;
            }
        }

        public void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            int column = textureId % (texture.Height / tileSize);
            int row = textureId / (texture.Width / tileSize);
            
            Rectangle tilesetRec = new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
            spriteBatch.Draw(texture, new Rectangle((int)tempX, (int)tempY, tileSize, tileSize), tilesetRec, Color.White);
        }
    }
}
