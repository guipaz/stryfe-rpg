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
using StryfeCore.Network.SharedModels;
using StryfeRPG.System.Runtime;

namespace StryfeRPG.Managers
{
    public class MapManager
    {
        public SpriteBatch spriteBatch { get; set; }
        public Map currentMap;

        private bool mustTeleport = false;
        private Teleport teleportObject;

        // Networking
        public Dictionary<int, Player> visiblePlayers = new Dictionary<int, Player>();

        public void PerformAction(Vector2 position)
        {
            MapObject obj = currentMap.GetObject(position);
            if (obj != null)
            {
                obj.PerformAction();
            }
        }

        public void Teleport(Teleport teleport)
        {
            Teleport(teleport.Map, teleport.TeleportPosition, teleport.Direction);
        }

        public void Teleport(string map, Vector2 position, FacingDirection direction)
        {
            LoadMap(map);
            Global.Player.MapPosition = position;
            Global.Player.Direction = direction;
        }

        public bool GetCollision(Vector2 movement)
        {
            // Checks map collision
            int collision = currentMap.GetCollision(movement);

            // Checks player collision
            collision = Global.Player.MapPosition == movement ? 1 : collision;

            // Checks NPC collision
            MapObject obj = currentMap.GetObject(movement);
            if (obj != null)
            {
                if (obj is Teleport == false)
                    collision = obj.MapPosition == movement ? 1 : collision;
            }

            return collision == 1;
        }

        public void LoadMap(string mapName)
        {
            TmxMap tmxMap = new TmxMap(String.Format("Content/Maps/{0}.tmx", mapName));
            currentMap = new Map(tmxMap, mapName);

            // Loads player
            if (currentMap.PlayerReference != null)
            {
                Global.Player = currentMap.PlayerReference;
            }

            // Plays the song
            if (currentMap.song != null)
            {
                AudioManager.Instance.PlaySong(currentMap.song);
            }
        }

        public void Update(double timePassed)
        {
            // Update Player
            UpdateObject(Global.Player, timePassed);

            // Update other players
            foreach (Player p in Global.NetPlayers.Values)
                UpdateObject(p, timePassed);

            // Update NPCs
            foreach (MapObject obj in currentMap.Objects)
            {
                if (obj.SavedInformation.IsActive)
                    UpdateObject(obj, timePassed);
            }

            if (mustTeleport)
            {
                mustTeleport = false;
                Teleport(teleportObject);
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
                    mustTeleport = currentMap.GetCollision(mapPosition) == 2;

                    if (mustTeleport)
                        teleportObject = (Teleport)currentMap.GetObject(mapPosition);
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
            foreach (TmxLayer layer in currentMap.TmxMap.Layers)
            {
                if (!currentMap.AboveLayers.Contains(layer))
                {
                    DrawMapLayer(layer);
                }
            }

            // Draw the player
            DrawObject(Global.Player);

            // Draw the other players
            foreach (Player p in visiblePlayers.Values)
            {
                DrawObject(p);
            }

            // Draw above tiles of the map
            foreach (TmxLayer layer in currentMap.AboveLayers)
            {
                DrawMapLayer(layer);
            }

            //Draw the Objects
            foreach (MapObject obj in currentMap.Objects)
            {
                if (obj.SavedInformation.IsActive)
                {
                    DrawObject(obj);
                }
            }

            // Draw the Objects names
            foreach (MapObject obj in currentMap.Objects)
            {
                if (obj is NPC)
                    DrawObjectName(obj, spriteBatch);
            }

            // Draw the player's name
            //DrawObjectName(Global.Player, spriteBatch);
        }

        private void DrawObject(MapObject obj)
        {
            int size = Global.TileSize;
            int textureId = obj.GetSprite();

            Texture2D texture = obj.Texture;
            Vector2 currentPosition = obj.CurrentPosition;

            spriteBatch.Draw(texture, new Rectangle((int)currentPosition.X, (int)currentPosition.Y, size, size), Utils.GetRectangleByGid(textureId, size, texture.Width), Color.White);
        }

        private void DrawObjectName(MapObject obj, SpriteBatch spriteBatch)
        {
            // Draw the name
            if (obj.Name != null && obj.SavedInformation.IsActive)
            {
                Vector2 textSize = Global.MapFont.MeasureString(obj.Name);
                int zoom = (int)CameraManager.Instance.Zoom;
                spriteBatch.DrawString(Global.MapFont, obj.Name, new Vector2(obj.CurrentPosition.X + (Global.TileSize / 2) - (textSize.X / 2), obj.CurrentPosition.Y - Global.MapFont.LineSpacing) + new Vector2(1, 1), new Color(Color.Black, 0.5f));
                spriteBatch.DrawString(Global.MapFont, obj.Name, new Vector2(obj.CurrentPosition.X + (Global.TileSize / 2) - (textSize.X / 2), obj.CurrentPosition.Y - Global.MapFont.LineSpacing), obj.NameColor);
            }
        }

        private void DrawMapLayer(TmxLayer layer)
        {
            TmxMap map = currentMap.TmxMap;
            int size = Global.TileSize;

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

        public void UpdatePlayer(PlayerInfo info)
        {
            if (!visiblePlayers.ContainsKey(info.id))
                visiblePlayers[info.id] = new Player(Global.defaultObj, Global.defaultTileset); //TODO
            visiblePlayers[info.id].SetInfo(info);
        }

        public void UpdatePlayers(List<PlayerInfo> players)
        {
            visiblePlayers.Clear();

            foreach (PlayerInfo info in players)
            {
                if (info.id == GameState.Instance.playerInfo.id)
                    continue;

                //TODO player factory
                Player p = new Player(Global.defaultObj, Global.defaultTileset);
                p.SetInfo(info);
                visiblePlayers[p.id] = p;
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
