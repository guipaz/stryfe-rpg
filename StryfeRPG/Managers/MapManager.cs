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
using StryfeRPG.System;

namespace StryfeRPG.Managers
{
    public class MapManager
    {
        public SpriteBatch spriteBatch { get; set; }
        public Map currentMap;
        
        public void LoadMap(string mapName)
        {
            TmxMap map = new TmxMap(String.Format("Content/Maps/{0}.tmx", mapName));
            Texture2D tileset = Global.GetTexture(map.Tilesets[0].Name.ToString());

            currentMap = new Map(map, tileset);
            currentMap.PopulateCollisions();
        }

        public void Update(double timePassed)
        {
            // Update Player
            UpdateObject(Player.Instance, timePassed);

            // Update NPCs
            foreach (MapObject obj in currentMap.npcs)
            {
                UpdateObject(obj, timePassed);
            }
        }
        
        private void UpdateObject(MapObject obj, double timePassed)
        {
            int size = Global.tileSize;
            Vector2 currentPosition = obj.currentPosition;
            Vector2 destinationPosition = obj.destinationPosition;
            Vector2 mapPosition = obj.mapPosition;
            double lerpTime = obj.lerpTime;

            currentPosition = mapPosition * size;

            // Calculate moving animation
            if (obj.isMoving)
            {
                int x = (int)(currentPosition.X + (destinationPosition.X - currentPosition.X) * lerpTime);
                int y = (int)(currentPosition.Y + (destinationPosition.Y - currentPosition.Y) * lerpTime);
                currentPosition = new Vector2(x, y);
                lerpTime += timePassed * obj.animationSpeed;

                // Finished movement
                if (lerpTime >= 1)
                {
                    lerpTime = 0;
                    obj.isMoving = false;
                    mapPosition = destinationPosition / size;
                }
            }

            obj.currentPosition = currentPosition;
            obj.destinationPosition = destinationPosition;
            obj.mapPosition = mapPosition;
            obj.lerpTime = lerpTime;
        }

        public void Draw(double timePassed)
        {
            // Draw the map
            DrawMap();

            // Draw the player
            DrawObject(Player.Instance);

            //Draw the NPCs
            foreach (Character npc in currentMap.npcs)
            {
                DrawObject(npc);
            }
        }

        private void DrawObject(MapObject obj)
        {
            int size = Global.tileSize;
            int textureId = obj.textureId;
            Texture2D texture = obj.texture;
            Vector2 currentPosition = obj.currentPosition;

            int column = textureId % (texture.Height / Global.tileSize);
            int row = textureId / (texture.Width / Global.tileSize);

            Rectangle tilesetRec = new Rectangle(size * column, size * row, size, size);
            spriteBatch.Draw(texture, new Rectangle((int)currentPosition.X, (int)currentPosition.Y, size, size), tilesetRec, Color.White);
        }
        
        private void DrawMap()
        {
            TmxMap map = currentMap.tmxMap;
            int size = Global.tileSize;

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

                        Rectangle tilesetRec = new Rectangle(size * column, size * row, size, size);
                        spriteBatch.Draw(currentMap.tileset, new Rectangle((int)x, (int)y, size, size), tilesetRec, Color.White);
                    }
                }
            }
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
