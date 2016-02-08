using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class DialogManager
    {
        public string currentText { get; set; } //TODO: change to a dialog object

        private int margin = 25;

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw dialog box
            Texture2D texture = Global.GetTexture("dialog_bg");
            Rectangle bounds = Global.Viewport.Bounds;

            int yOffset = bounds.Height / 2 + 25;

            spriteBatch.Draw(texture,
                             destinationRectangle: new Rectangle(0, yOffset, bounds.Width, bounds.Height / 2 + 25),
                             color: new Color(Color.White, 0.8f));

            // Draw text
            spriteBatch.DrawString(Global.DialogFont, currentText, new Vector2(margin + 1, yOffset + margin + 1), Color.Black);
            spriteBatch.DrawString(Global.DialogFont, currentText, new Vector2(margin + 2, yOffset + margin + 2), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(Global.DialogFont, currentText, new Vector2(margin, yOffset + margin), Color.White);
        }

        public void SetCurrentText(string text)
        {
            this.currentText = text;

            // Calculates the text area
            Rectangle bounds = Global.Viewport.Bounds;
            float maximumWidth = bounds.Width - (margin * 2);
            string finalString = "";

            while (currentText.Count() > 0)
            {
                Vector2 measure = Global.DialogFont.MeasureString(currentText);
                if (measure.X > maximumWidth)
                {
                    int charsThatFit = (int)(currentText.Count() * maximumWidth / measure.X) - 1;
                    string currentWithBreak = currentText.Insert(charsThatFit, "\n");
                    finalString += currentWithBreak.Substring(0, currentWithBreak.IndexOf("\n") + 1);
                    currentText = currentWithBreak.Substring(currentWithBreak.IndexOf("\n") + 1).Trim();
                } else
                {
                    finalString += currentText;
                    break;
                }
            }

            currentText = finalString;
        }

        // Singleton stuff
        private static DialogManager instance;
        protected DialogManager() { }
        public static DialogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DialogManager();
                }
                return instance;
            }
        }
    }
}
