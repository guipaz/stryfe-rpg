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

            currentMap = new Map();

            currentMap.tmxMap = map;
            currentMap.tileset = tileset;

            currentMap.tileWidth = map.Tilesets[0].TileWidth;
            currentMap.tileHeight = map.Tilesets[0].TileHeight;

            currentMap.tilesetTilesWide = tileset.Width / currentMap.tileWidth;
            currentMap.tilesetTilesHigh = tileset.Height / currentMap.tileHeight;
        }

        public void Draw()
        {
            DrawMap();
            DrawPlayer();
        }

        private void DrawMap()
        {
            TmxMap map = currentMap.tmxMap;

            spriteBatch.Begin();

            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;
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

            spriteBatch.End();
        }

        private void DrawPlayer()
        {
            spriteBatch.Begin();

            Player.Instance.Draw(spriteBatch);

            spriteBatch.End();
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
