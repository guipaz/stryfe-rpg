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
        
        public void PerformAction(Vector2 position)
        {
            MapObject obj = currentMap.GetObject(position);
            if (obj != null)
            {
                obj.PerformAction();
            }
        }

        public void LoadMap(string mapName)
        {
            TmxMap tmxMap = new TmxMap(String.Format("Content/Maps/{0}.tmx", mapName));
            currentMap = new Map(tmxMap);

            // Loads player
            if (currentMap.PlayerReference != null)
            {
                Global.Player = currentMap.PlayerReference;
            }
        }

        public void Update(double timePassed)
        {
            // Update Player
            UpdateObject(Global.Player, timePassed);

            // Update NPCs
            foreach (MapObject obj in currentMap.Npcs)
            {
                UpdateObject(obj, timePassed);
            }
        }
        
        private void UpdateObject(MapObject obj, double timePassed)
        {
            int size = Global.TileSize;
            Vector2 currentPosition = obj.CurrentPosition;
            Vector2 destinationPosition = obj.DestinationPosition;
            Vector2 mapPosition = obj.MapPosition;
            double lerpTime = obj.LerpTime;

            currentPosition = mapPosition * size;

            // Calculate moving animation
            if (obj.IsMoving)
            {
                int x = (int)(currentPosition.X + (destinationPosition.X - currentPosition.X) * lerpTime);
                int y = (int)(currentPosition.Y + (destinationPosition.Y - currentPosition.Y) * lerpTime);
                currentPosition = new Vector2(x, y);
                lerpTime += timePassed * obj.AnimationSpeed;

                // Finished movement
                if (lerpTime >= 1)
                {
                    lerpTime = 0;
                    obj.IsMoving = false;
                    mapPosition = destinationPosition / size;
                }
            }

            obj.CurrentPosition = currentPosition;
            obj.DestinationPosition = destinationPosition;
            obj.MapPosition = mapPosition;
            obj.LerpTime = lerpTime;
        }

        public void Draw(double timePassed)
        {
            // Draw the map
            DrawMap();

            // Draw the player
            DrawCharacter(Global.Player);

            //Draw the NPCs
            foreach (Character npc in currentMap.Npcs)
            {
                DrawCharacter(npc);
            }
        }

        private void DrawCharacter(Character obj)
        {
            int size = Global.TileSize;
            int textureId = obj.GetSprite();

            Texture2D texture = obj.Texture;
            Vector2 currentPosition = obj.CurrentPosition;

            int tilesWide = (texture.Width / Global.TileSize);
            int column = textureId % tilesWide;
            int row = textureId / tilesWide;

            Rectangle tilesetRec = new Rectangle(size * column, size * row, size, size);
            spriteBatch.Draw(texture, new Rectangle((int)currentPosition.X, (int)currentPosition.Y, size, size), tilesetRec, Color.White);
        }
        
        private void DrawMap()
        {
            TmxMap map = currentMap.TmxMap;
            int size = Global.TileSize;

            foreach (TmxLayer layer in map.Layers)
            {
                for (var i = 0; i < layer.Tiles.Count; i++)
                {
                    int gid = layer.Tiles[i].Gid;
                    if (gid != 0)
                    {
                        Tileset tileset = currentMap.GetTileset(gid);

                        int tileFrame = gid - tileset.FirstGid;
                        int column = tileFrame % tileset.TilesWide;
                        int row = tileFrame / tileset.TilesWide;

                        float x = (i % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;
                        
                        Rectangle tilesetRec = new Rectangle(size * column, size * row, size, size);
                        spriteBatch.Draw(tileset.Texture, new Rectangle((int)x, (int)y, size, size), tilesetRec, Color.White);
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
