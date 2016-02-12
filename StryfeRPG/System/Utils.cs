using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StryfeRPG.Managers;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
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
        
        public static void LoadDialogs()
        {
            Global.SetDialogs(JsonConvert.DeserializeObject<Dictionary<int, Dialog>>(File.ReadAllText("Content/Data/dialogs.json")));
        }

        public static void LoadScripts()
        {
            Global.SetScripts(JsonConvert.DeserializeObject<Dictionary<int, Script>>(File.ReadAllText("Content/Data/scripts.json")));
        }

        public static void LoadItems()
        {
            Global.SetItems(JsonConvert.DeserializeObject<Dictionary<int, Item>>(File.ReadAllText("Content/Data/items.json")));
        }

        public static FacingDirection GetDirection(string str)
        {
            switch (str)
            {
                case "up":
                    return FacingDirection.Up;
                case "left":
                    return FacingDirection.Left;
                case "right":
                    return FacingDirection.Right;
            }

            return FacingDirection.Down;
        }
    }
}
