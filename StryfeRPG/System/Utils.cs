using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StryfeRPG.Managers;
using StryfeRPG.Models.Maps;
using StryfeRPG.Models.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StryfeRPG.System
{
    public static class Utils
    {
        public static void DrawText(SpriteFont font, SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(font, text, position + new Vector2(2, 2), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(font, text, position, color);
        }

        public static bool GetCollision(Vector2 movement)
        {
            Map map = MapManager.Instance.currentMap;

            // Checks map collision
            int collision = map.GetCollision(movement);

            // Checks player collision
            collision = Global.Player.MapPosition == movement ? 1 : collision;

            // Checks NPC collision
            foreach (MapObject obj in map.Objects)
            {
                if (obj is Teleport == false)
                    collision = obj.MapPosition == movement ? 1 : collision;
            }

            return collision == 1;
        }

        public static void LoadDialogs()
        {
            Global.SetDialogs(JsonConvert.DeserializeObject<Dictionary<int, Dialog>>(File.ReadAllText("Content/Data/dialogs.json")));
        }
    }
}
