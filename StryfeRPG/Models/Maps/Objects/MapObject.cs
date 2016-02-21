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
        // Basic information
        public string Identifier { get; set; }
        public string Name { get; set; }
        public Color NameColor { get; set; }
        public int ScriptId { get; set; }
        public ObjectInfo SavedInformation { get; set; }

        // Texture stuff
        public Texture2D Texture { get; set; }
        public int TextureId { get; set; }

        // Positioning stuff (tile-based)
        public Vector2 MapPosition { get; set; }
        public Vector2 Size { get; set; }

        // Moving stuff (positions are pixel-based)
        public bool IsMoving { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 DestinationPosition { get; set; }
        public double LerpTime = 0;
        public double AnimationSpeed = 5;

        public MapObject(TmxObject obj, Tileset tileset, string mapName = "")
        {
            // Name
            Name = obj.Name != null ? obj.Name : "NoName";

            // Identifier used for ObjectInfo instance
            Identifier = String.Format("{0}_{1}_{2}", obj.Id, mapName, Name).Replace(" ", string.Empty);

            // ObjectInfo instance, holding information about the object in runtime
            SavedInformation = new ObjectInfo(Identifier);
            if (obj.Properties.ContainsKey("active"))
            {
                SavedInformation.IsActive = obj.Properties["active"] == "true" ? true : false;
            }

            // Gets the script id
            ScriptId = obj.Properties.ContainsKey("script") ? int.Parse(obj.Properties["script"]) : -1;

            // Gets the name color
            if (obj.Properties.ContainsKey("color"))
            {
                string[] rgba = obj.Properties["color"].Split(',');
                NameColor = rgba.Count() == 4 ? new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3])) : Color.White;
            } else
            {
                NameColor = Color.White;
            }

            // Gets texture information
            Texture = tileset.Texture;
            TextureId = obj.Tile != null ? obj.Tile.Gid - tileset.FirstGid : -1;

            // Workaround for the Y axis when it's a tile object (instead of rectangle)
            int y = obj.ObjectType == TmxObjectType.Tile ? (int)obj.Y - 1 : (int)obj.Y;

            // Sets position
            MapPosition = new Vector2((int)obj.X / Global.TileSize, y / Global.TileSize);
            CurrentPosition = MapPosition * Global.TileSize;
            DestinationPosition = CurrentPosition;
            IsMoving = false;

            // Width and height for objects bigger than a tile
            Size = new Vector2((int)obj.Width / Global.TileSize, (int)obj.Height / Global.TileSize);
        }

        public bool Move(Vector2 movement)
        {
            IsMoving = true;
            DestinationPosition = (MapPosition + movement) * Global.TileSize;

            return true;
        }

        public virtual void PerformAction()
        {
            if (ScriptId != -1)
                ScriptInterpreter.Instance.RunScript(ScriptId, this);

            SavedInformation.NumberOfInteractions++;
        }

        public virtual void Dismiss()
        {
            //TODO
        }

        public virtual int GetSprite()
        {
            return TextureId;
        }
    }
}
