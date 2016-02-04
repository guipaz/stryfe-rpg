using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StryfeRPG.Models.Maps;
using TiledSharp;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using StryfeRPG.Models.Characters;

namespace StryfeRPG.Managers
{
    public class MapManager
    {
        public ContentManager Content { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public Map currentMap;
        
        public void LoadMap(string mapName)
        {
            TmxMap map = new TmxMap(String.Format("Content/Maps/{0}.tmx", mapName));
            Texture2D tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());

            currentMap = new Map(map, tileset);
            currentMap.UpdateCollisions();
        }

        public void Draw(double timePassed)
        {
            DrawMap();
            DrawCharacters(timePassed);
        }

        private void DrawMap()
        {
            TmxMap map = currentMap.tmxMap;
            
            foreach (TmxLayer layer in map.Layers)
            {
                for (var i = 0; i < layer.Tiles.Count; i++)
                {
                    int gid = layer.Tiles[i].Gid;
                    if (gid != 0)
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % currentMap.tilesetTilesWide;
                        int row = tileFrame / currentMap.tilesetTilesWide;

                        float x = (i % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(currentMap.tileWidth * column, currentMap.tileHeight * row, currentMap.tileWidth, currentMap.tileHeight);
                        spriteBatch.Draw(currentMap.tileset, new Rectangle((int)x, (int)y, currentMap.tileWidth, currentMap.tileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }

        private void DrawCharacters(double timePassed)
        {
            Player.Instance.Draw(spriteBatch, timePassed);
        }

        // Singleton stuff
        private static MapManager instance;
        protected MapManager() { }
        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }
    }
}
