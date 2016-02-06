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
        public string name { get; set; }

        // Texture stuff
        public Texture2D texture { get; set; }
        public int textureId { get; set; }

        // Positioning stuff (tile-based)
        public Vector2 mapPosition { get; set; }

        // Moving stuff (pixel-based)
        public bool isMoving { get; set; }
        
        public Vector2 currentPosition { get; set; }
        public Vector2 destinationPosition { get; set; }

        public double lerpTime = 0;
        public double animationSpeed = 5;

        public MapObject() { }
        public MapObject(TmxObject obj)
        {
            name = obj.Name != null ? obj.Name : "NoName";
            texture = Global.GetTexture("charsets"); //TODO
            textureId = obj.Tile.Gid - 85;
            mapPosition = new Vector2((int)obj.X / Global.tileSize, (int)(obj.Y - 1) / Global.tileSize);
            currentPosition = mapPosition * Global.tileSize;
            destinationPosition = currentPosition;
            isMoving = false;
        }

        public bool Move(Vector2 movement)
        {
            isMoving = true;
            destinationPosition = (mapPosition + movement) * Global.tileSize;

            return true;
        }
    }
}
