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
        public int destinationX { get; set; }
        public int destinationY { get; set; }
        double lerpTime = 0;
        double animationSpeed = 5;

        public void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            int column = textureId % (texture.Height / tileSize);
            int row = textureId / (texture.Width / tileSize);

            int finalPosX = positionX;
            int finalPosY = positionY;

            // Calculate moving animation
            if (isMoving)
            {
                finalPosX = (int)(positionX + (destinationX - positionX) * lerpTime);
                finalPosY = (int)(positionY + (destinationY - positionY) * lerpTime);
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

            Rectangle tilesetRec = new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
            spriteBatch.Draw(texture, new Rectangle((int)finalPosX, (int)finalPosY, tileSize, tileSize), tilesetRec, Color.White);
        }
    }
}
