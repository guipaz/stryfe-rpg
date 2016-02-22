using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System
{
    public static class WindowUtils
    {
        public static SpriteBatch spriteBatch;

        public static void DrawWindow(Window window, Rectangle? relativeBounds = null)
        {
            if (window is TextWindow)
            {
                TextWindow textWindow = (TextWindow)window;

                Vector2 position = Vector2.Zero;
                if (relativeBounds != null)
                    position = new Vector2(((Rectangle)relativeBounds).X + textWindow.Bounds.X, ((Rectangle)relativeBounds).Y + textWindow.Bounds.Y);
                else
                    position = new Vector2(textWindow.Bounds.X, textWindow.Bounds.Y);

                textWindow.RecalculateText();

                spriteBatch.DrawString(textWindow.Font, textWindow.Text,
                                       position,
                                       textWindow.Color);
            } else
            {
                spriteBatch.Draw(window.BackgroundTexture,
                             destinationRectangle: relativeBounds != null ? relativeBounds : window.Bounds,
                             color: window.Color,
                             sourceRectangle: window.SourceRectangle != Rectangle.Empty ? (Rectangle?)window.SourceRectangle : null);
            }

            foreach (Window child in window.Children)
            {
                if (relativeBounds == null)
                    relativeBounds = window.Bounds;

                Rectangle relative = new Rectangle(((Rectangle)relativeBounds).X + child.Bounds.X + window.Margin,
                                                   ((Rectangle)relativeBounds).Y + child.Bounds.Y + window.Margin,
                                                   child.Bounds.Width,
                                                   child.Bounds.Height);
                DrawWindow(child, relative);
            }
                
        }
    }
}
