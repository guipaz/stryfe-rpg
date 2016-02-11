using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TiledSharp;

namespace StryfeRPG.Models.Maps
{
    public class Map
    {
        public string Name { get; set; }
        // The TmxMap with every information of the XML
        public TmxMap TmxMap { get; set; }

        // References for the 
        public List<Tileset> Tilesets { get; set; }
        public List<MapObject> Objects { get; set; } //TODO: maybe change it to generic MapObject
        public Player PlayerReference { get; set; }

        // A control array for collisions
        public int[] CollisionMap { get; set; }

        // Width and height in tiles
        public int Width { get; set; }
        public int Height { get; set; }

        // The layers that are supposed to be rendered after the player
        public List<TmxLayer> AboveLayers;

        // The song name for the map
        public string song { get; set; }

        public Map(TmxMap tmxMap, string name)
        {
            this.TmxMap = tmxMap;
            Name = name;

            Width = tmxMap.Width;
            Height = tmxMap.Height;

            Tilesets = new List<Tileset>();
            foreach (TmxTileset tmxTileset in tmxMap.Tilesets)
            {
                Tilesets.Add(new Tileset(tmxTileset));
            }

            Objects = new List<MapObject>();
            foreach (TmxObjectGroup group in tmxMap.ObjectGroups)
            {
                foreach (TmxObject obj in group.Objects)
                {
                    if (obj.Type == "npc")
                    {
                        Objects.Add(new Character(obj, GetTileset(obj.Tile != null ? obj.Tile.Gid : -1), Name));
                    } else if (obj.Type == "player")
                    {
                        PlayerReference = new Player(obj, GetTileset(obj.Tile != null ? obj.Tile.Gid : -1));
                    } else if (obj.Type == "teleport")
                    {
                        Objects.Add(new Teleport(obj, GetTileset(obj.Tile != null ? obj.Tile.Gid : -1)));
                    }
                }
            }

            // Populate above layers
            AboveLayers = new List<TmxLayer>();
            foreach (TmxLayer layer in tmxMap.Layers)
            {
                if (layer.Properties.ContainsKey("above") && layer.Properties["above"] == "true")
                    AboveLayers.Add(layer);
            }

            // Gets the song
            if (TmxMap.Properties.ContainsKey("song"))
            {
                song = TmxMap.Properties["song"];
            }

            PopulateCollisions();
        }

        public void PopulateCollisions()
        {
            // Populate collisions
            CollisionMap = new int[Width * Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // Populate teleports
                    MapObject obj = GetObject(new Vector2(x, y));
                    if (obj is Teleport)
                    {
                        CollisionMap[y * Width + x] = 2;
                    }
                    
                    // Populate collisions
                    foreach (TmxLayer layer in TmxMap.Layers)
                    {
                        if (layer.Tiles[y * Width + x].Gid != 0 &&
                            layer.Properties.ContainsKey("collision") &&
                            layer.Properties["collision"] == "true")
                        {
                            CollisionMap[y * Width + x] = 1;
                        }
                    }
                }
            }
        }

        public int GetCollision(Vector2 movement)
        {
            if (movement.X >= 0 && movement.Y >= 0 && movement.X < Width && movement.Y < Height)
                return CollisionMap[(int)movement.Y * Width + (int)movement.X];
            return 1;
        }

        public Tileset GetTileset(int gid)
        {
            foreach (Tileset tileset in Tilesets)
            {
                if (gid >= tileset.FirstGid && gid <= tileset.FinalGid)
                {
                    return tileset;
                }
            }

            return Tilesets[0];
        }

        public MapObject GetObject(Vector2 position)
        {
            foreach (MapObject obj in Objects)
            {
                if (position.X >= obj.MapPosition.X && position.X <= obj.MapPosition.X + obj.Size.X - 1 &&
                    position.Y >= obj.MapPosition.Y && position.Y <= obj.MapPosition.Y + obj.Size.Y - 1)
                    return obj;
            }

            return null;
        }
    }
}
