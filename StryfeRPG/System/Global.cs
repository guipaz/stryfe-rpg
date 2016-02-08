using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.System
{
    public static class Global
    {
        public static Player Player;
        public static int TileSize = 32;
        public static SpriteFont DefaultFont;

        private static ContentManager Content;
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        
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
    }
}
