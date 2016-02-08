using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Managers;
using StryfeRPG.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.System
{
    public static class Utils
    {
        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(Global.MapFont, text, position + new Vector2(1, 1), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(Global.MapFont, text, position, color);
        }

        public static bool GetCollision(Vector2 movement)
        {
            Map map = MapManager.Instance.currentMap;

            // Checks map collision
            int collision = map.GetCollision(movement);

            // Checks player collision
            collision = Global.Player.mapPosition == movement ? 1 : collision;

            // Checks NPC collision
            foreach (MapObject obj in map.npcs)
            {
                collision = obj.mapPosition == movement ? 1 : collision;
            }

            return collision == 1;
        }
    }
}
