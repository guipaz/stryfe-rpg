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

        public void Draw(SpriteBatch spriteBatch)
        {
            int column = textureId % (texture.Height / tileSize);
            int row = textureId / (texture.Width / tileSize);
            
            Rectangle tilesetRec = new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
            spriteBatch.Draw(texture, new Rectangle((int)positionX, (int)positionY, tileSize, tileSize), tilesetRec, Color.White);
        }
    }
}
