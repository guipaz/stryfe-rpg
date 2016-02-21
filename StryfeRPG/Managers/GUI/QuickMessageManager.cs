using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeCore.Models.Utils;

namespace StryfeRPG.Managers.GUI
{
    public class QuickMessageManager
    {
        public List<QuickMessage> Messages;

        private QuickMessage currentMessage;

        // Time control
        private double cooldown = 1;
        private double timeLeft = 0;

        // Screen
        private Texture2D texture;
        private Rectangle bounds;
        private int lineHeight;

        public void ShowMessage(QuickMessage message)
        {
            Messages.Add(message);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // If there's no message, don't draw anything
            if (currentMessage == null)
                return;

            // Measurement and drawing
            Vector2 titleMeasure = Global.DialogFont.MeasureString(currentMessage.Title);
            int titleHeight = (int)titleMeasure.Y;
            int margin = 20;
            int width = (int)titleMeasure.X;

            Vector2 messageMeasure = Global.DetailFont.MeasureString(currentMessage.Message);
            int messageHeight = (int)messageMeasure.Y;

            if (messageMeasure.X > width)
                width = (int)messageMeasure.X;

            width += margin * 2;
            int height = titleHeight + messageHeight + margin * 3;

            int windowX = bounds.Width / 2 - width / 2;
            int windowY = bounds.Height / 2 - height / 2 - 100;
            spriteBatch.Draw(texture, new Rectangle(windowX, windowY, width, height), new Color(Color.White, 0.8f));

            // Title
            int textX = windowX + width / 2 - (int)titleMeasure.X / 2;
            int textY = windowY + margin;
            spriteBatch.DrawString(Global.DialogFont, currentMessage.Title, new Vector2(textX, textY), Color.Yellow);

            // Message
            textX = windowX + width / 2 - (int)messageMeasure.X / 2;
            textY += (int)messageMeasure.Y + margin;
            spriteBatch.DrawString(Global.DetailFont, currentMessage.Message, new Vector2(textX, textY), Color.White);
        }

        public void Update(double timePassed)
        {
            // If there's no message, just return
            if (Messages.Count() == 0 && currentMessage == null)
                return;

            // Checks if there's a message being shown
            if (timeLeft > 0)
            {
                timeLeft -= timePassed;
                return;
            }
            else if (timeLeft < 0)
            {
                timeLeft = 0;
            }

            // Unreference the shown message
            currentMessage = null;

            // If there's no message left, return
            if (Messages.Count() == 0)
                return;

            // Sets the message that will be shown, the cooldown and removes from the queue
            currentMessage = Messages[0];
            Messages.RemoveAt(0);
            timeLeft = cooldown;
        }

        // Singleton stuff
        private static QuickMessageManager instance;

        protected QuickMessageManager()
        {
            texture = Global.GetTexture("dialog_bg");
            bounds = Global.Viewport.Bounds;

            lineHeight = Global.DialogFont.LineSpacing;
            Messages = new List<QuickMessage>();
        }
        public static QuickMessageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QuickMessageManager();
                }
                return instance;
            }
        }
    }
}
