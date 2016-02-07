using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            spriteBatch.DrawString(Global.defaultFont, text, position + new Vector2(1, 1), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(Global.defaultFont, text, position, color);
        }
    }
}
