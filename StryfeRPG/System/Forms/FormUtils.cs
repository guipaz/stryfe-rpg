using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Forms
{
    public static class FormUtils
    {
        public static GraphicsDevice device { get; set; }

        public static Texture2D GetSolidTexture(Color c)
        {
            Texture2D rect = new Texture2D(device, 80, 30);

            Color[] data = new Color[80 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = c;
            rect.SetData(data);

            return rect;
        }
    }
}
