using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Windows
{
    public class Window
    {
        public Rectangle Bounds { get; set; }
        public Texture2D BackgroundTexture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Color Color { get; set; }
        public List<Window> Children { get; set; }
        public int Margin { get; set; }

        public Window(int x, int y, int width, int height, Texture2D texture = null, Color? color = null, Rectangle? sourceRect = null)
        {
            Bounds = new Rectangle(x, y, width, height);
            BackgroundTexture = texture != null ? texture : Global.GetTexture("dialog_bg");
            Color = color != null ? (Color)color : new Color(Color.White, 0.8f);
            SourceRectangle = sourceRect != null ? (Rectangle)sourceRect : Rectangle.Empty;

            Children = new List<Window>();
        }

        public void AddChild(Window w)
        {
            Children.Add(w);
        }
    }
}
