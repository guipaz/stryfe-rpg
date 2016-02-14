using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers.Data
{
    public abstract class WindowManager
    {
        public static WindowManager CurrentManager { get; set; }
        public static bool IsWindowOpened { get; private set; }

        public bool IsOpened { get; protected set; }
        protected int Width = 550;
        protected int Height = 300;

        protected Texture2D bgTexture;
        protected Rectangle bounds;

        public void ToggleWindow()
        {
            if (IsOpened)
                CloseWindow();
            else
                OpenWindow();
        }

        public virtual void OpenWindow()
        {
            IsOpened = true;

            CurrentManager = this;
            IsWindowOpened = true;
        }

        public virtual void CloseWindow()
        {
            IsOpened = false;

            CurrentManager = null;
            IsWindowOpened = false;
        }

        public abstract void Draw(SpriteBatch spriteBatch, double timePassed);
        public abstract void PerformAction();
    }
}
