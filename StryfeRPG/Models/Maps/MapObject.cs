using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Managers;
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
        public string Name { get; set; }
        public Color NameColor { get; set; }

        // Texture stuff
        public Texture2D Texture { get; set; }
        public int TextureId { get; set; }

        // Positioning stuff (tile-based)
        public Vector2 MapPosition { get; set; }

        // Moving stuff (pixel-based)
        public bool IsMoving { get; set; }
        
        public Vector2 CurrentPosition { get; set; }
        public Vector2 DestinationPosition { get; set; }

        public double LerpTime = 0;
        public double AnimationSpeed = 5;

        public MapObject() { }
        public MapObject(TmxObject obj, Tileset tileset)
        {
            Name = obj.Name != null ? obj.Name : "NoName";
            NameColor = Color.Yellow; //TODO

            Texture = tileset.Texture;
            TextureId = obj.Tile.Gid - tileset.FirstGid;

            MapPosition = new Vector2((int)obj.X / Global.TileSize, (int)(obj.Y - 1) / Global.TileSize);
            CurrentPosition = MapPosition * Global.TileSize;
            DestinationPosition = CurrentPosition;
            IsMoving = false;
        }

        public bool Move(Vector2 movement)
        {
            IsMoving = true;
            DestinationPosition = (MapPosition + movement) * Global.TileSize;

            return true;
        }

        public virtual void PerformAction()
        {
            //TODO
        }

        public virtual void Dismiss()
        {
            //TODO
        }
    }
}
