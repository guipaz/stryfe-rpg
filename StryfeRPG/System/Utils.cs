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
            List<Item> itemsJson = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText("Content/Data/items.json"));
            Dictionary<int, Item> items = new Dictionary<int, Item>();
            foreach (Item i in itemsJson)
                items[i.Id] = i;
            Global.SetItems(items);
        }

        // [0] is the final string, [1] is the remaining (if there's any)
        public static string[] GetCroppedString(string text, SpriteFont font, float maxWidth, float maxHeight)
        {
            string editText = text;
            string finalString = "";

            // Iterates the whole text for horizontal adjust
            Vector2 measure;
            while (editText.Count() > 0)
            {
                measure = font.MeasureString(editText);
                if (measure.X > maxWidth)
                {
                    // Gets how many characters fit in the screen
                    int charsThatFit = (int)(editText.Count() * maxWidth / measure.X) - 1;

                    // Gets the index of the last space so we put a break there
                    int breakIndex = editText.LastIndexOf(' ', charsThatFit, charsThatFit);

                    // Adds the break
                    string currentWithBreak = editText.Insert(breakIndex, "\n");

                    // Adds this line to the finalString and remove it from the rest of the text that will be examined
                    finalString += currentWithBreak.Substring(0, currentWithBreak.IndexOf("\n") + 1);
                    editText = currentWithBreak.Substring(currentWithBreak.IndexOf("\n") + 1).Trim();
                }
                else
                {
                    finalString += editText;
                    break;
                }
            }

            // Iterates the words for height fit, if needed
            measure = font.MeasureString(finalString);
            int cutIndex = 0;
            string originalString = finalString;
            char[] chars = new char[2] { ' ', '\n' };

            while (measure.Y > maxHeight)
            {
                cutIndex = finalString.LastIndexOfAny(chars, finalString.Count() - 1, finalString.Count() - 1);

                finalString = finalString.Substring(0, cutIndex);
                measure = font.MeasureString(finalString);
            }

            // Sets the final variables for use
            string remaining = originalString.Substring(cutIndex);
            if (remaining.Count() > 0 && remaining[0] == '\n')
                remaining = remaining.Substring(1);
            remaining = remaining.Count() != finalString.Count() ? remaining : null;
            editText = finalString;

            string[] ret = new string[2] { editText, remaining };
            return ret;
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

        public static bool IsMenuOpened() // Inventory, quests, equipments, etc
        {
            return InventoryManager.Instance.IsOpened;
        }

        public static Rectangle GetRectangleByGid(int gid, int tileSize, int textureWidth)
        {
            int tilesWide = textureWidth / tileSize;
            int column = gid % tilesWide;
            int row = gid / tilesWide;

            return new Rectangle(tileSize * column, tileSize * row, tileSize, tileSize);
        }
    }
}
