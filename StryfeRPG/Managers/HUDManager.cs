using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Maps;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class HUDManager
    {
        private Texture2D DialogTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player's name (top)
            Vector2 measure = Global.DialogFont.MeasureString(Global.Player.Name);
            spriteBatch.Draw(DialogTexture, destinationRectangle: new Rectangle(0, 0, (int)measure.X + 20, (int)measure.Y + 20), color: new Color(Color.White, 0.8f));
            spriteBatch.DrawString(Global.DialogFont, Global.Player.Name, new Vector2(11, 11), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(Global.DialogFont, Global.Player.Name, new Vector2(10, 10), Color.White);
        }
        
        // Singleton stuff
        private static HUDManager instance;
        protected HUDManager()
        {
            DialogTexture = Global.GetTexture("dialog_bg");
        }
        public static HUDManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HUDManager();
                }
                return instance;
            }
        }
    }
}
