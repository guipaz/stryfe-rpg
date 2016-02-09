using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.System
{
    public static class Global
    {
        public static Viewport Viewport;
        public static Player Player;
        public static int TileSize = 64;
        public static SpriteFont MapFont;
        public static SpriteFont DialogFont;

        private static ContentManager Content;
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static Dictionary<int, Dialog> dialogs = new Dictionary<int, Dialog>();
        
        public static void SetContent(ContentManager content)
        {
            Content = content;
        }

        public static Texture2D GetTexture(string name)
        {
            if (!textures.ContainsKey(name))
                textures.Add(name, Content.Load<Texture2D>(name));
            return textures[name];
        }

        public static Dialog GetDialog(int id)
        {
            if (dialogs.ContainsKey(id))
                return dialogs[id];
            return null;
        }

        public static void SetDialogs(Dictionary<int, Dialog> dialogs)
        {
            Global.dialogs = dialogs;
        }
    }
}
