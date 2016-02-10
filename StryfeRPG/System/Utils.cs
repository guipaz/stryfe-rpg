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
