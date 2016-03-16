using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Forms
{
    public class SButton : SDrawable
    {
        Color color;
        string text;

        public SButton(Rectangle rect, Color color, string text)
        {
            this.rect = rect;
            this.color = color;
            this.text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(FormUtils.GetSolidTexture(color), rect, Color.White);

            SpriteFont font = Global.DetailFont;
            Vector2 size = font.MeasureString(text);
            spriteBatch.DrawString(font, text, new Vector2(rect.Center.X - size.X / 2, rect.Center.Y - size.Y / 2), Color.White);
        }
    }
}
