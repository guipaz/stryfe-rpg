using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Forms
{
    public abstract class SDrawable
    {
        protected Rectangle rect;

        public bool IsInside(Point position)
        {
            return rect.Contains(position);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
