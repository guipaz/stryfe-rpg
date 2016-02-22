using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Windows
{
    public class TextWindow : Window
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 MaxSize { get; set; }

        public TextWindow(SpriteFont font, string text, Vector2 position, Vector2 maxSize, Color? color = null) : base((int)position.X, (int)position.Y, 0, 0, null, color, null)
        {
            Text = text;
            Font = font;
            MaxSize = maxSize;
            RecalculateText();
        }

        public void RecalculateText()
        {
            Text = Utils.GetCroppedString(Text, Font, MaxSize.X, MaxSize.Y)[0];
        }
    }
}
