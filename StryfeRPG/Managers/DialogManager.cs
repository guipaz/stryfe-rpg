using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Models.Maps;
using StryfeRPG.Models.Utils;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class DialogManager
    {
        public Dialog currentDialog { get; set; }

        // Current message control
        private bool dialogActive = false;
        private int nextTextIndex = 0;
        private string currentText = "";
        private string remainingText = null;

        // Dialog box properties
        private Texture2D texture;
        private Rectangle bounds;
        private int lineHeight;
        private int yOffset;

        // Margin values
        private int marginX = 20;
        private int marginY = 15;

        // Animation control
        private float cooldown = 0;
        private float charsBySecond = 100;
        private bool isAnimating = false;
        private int animationIndex = 0;
        private string animatedText = "";

        // Reference for dismissing the object that called the dialog
        private MapObject dismissReference;

        public void ActivateDialog(Dialog dialog, MapObject dismissReference)
        {
            this.dismissReference = dismissReference;
            currentDialog = dialog;
            dialogActive = true;
            NextMessage();
        }

        public bool IsDialogActive()
        {
            return dialogActive && currentDialog != null;
        }

        public void NextMessage()
        {
            if (IsDialogActive())
            {
                // Resets animation index
                animationIndex = 0;
                animatedText = "";

                // If there's animation, just show the whole text
                if (isAnimating)
                {
                    isAnimating = false;
                    return;
                }
                
                // Checks if there's a remaining text
                if (remainingText != null && remainingText != "")
                {
                    SetCurrentText(remainingText);
                    remainingText = null;
                    return;
                }

                // Finishes the dialog
                if (nextTextIndex >= currentDialog.messages.Count())
                {
                    nextTextIndex = 0;
                    dialogActive = false;
                    currentDialog = null;
                    currentText = "";

                    if (dismissReference != null)
                        dismissReference.Dismiss();

                    dismissReference = null;
                } else // or shows the next message
                {
                    SetCurrentText(currentDialog.messages[nextTextIndex]);
                    nextTextIndex++;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            // Checks if there's a dialog active at the moment
            if (!IsDialogActive())
                return;
            
            // Set text according to the animation
            if (isAnimating)
            {
                if (cooldown > 0)
                {
                    cooldown -= (float)timePassed;
                } else
                {
                    cooldown = 1 / charsBySecond;
                    animatedText = currentText.Substring(0, animationIndex);
                    animationIndex++;

                    if (animationIndex >= currentText.Count())
                    {
                        isAnimating = false;
                        animationIndex = 0;
                    }
                }
            } else
            {
                animatedText = currentText;
            }

            // Draw dialog box
            spriteBatch.Draw(texture,
                             destinationRectangle: new Rectangle(0, bounds.Height - yOffset, bounds.Width, yOffset),
                             color: new Color(Color.White, 0.8f));

            // Draw text
            spriteBatch.DrawString(Global.DialogFont, animatedText, new Vector2(marginX + 1, bounds.Height - yOffset + marginY + 1), Color.Black);
            spriteBatch.DrawString(Global.DialogFont, animatedText, new Vector2(marginX + 2, bounds.Height - yOffset + marginY + 2), new Color(Color.Black, 0.5f));
            spriteBatch.DrawString(Global.DialogFont, animatedText, new Vector2(marginX, bounds.Height - yOffset + marginY), Color.White);
        }

        private void SetCurrentText(string text)
        {
            this.currentText = text;

            // Calculates the text area
            Rectangle bounds = Global.Viewport.Bounds;
            float maximumWidth = bounds.Width - (marginX * 2);
            string finalString = "";

            // Iterates the whole text for horizontal adjust
            Vector2 measure;
            while (currentText.Count() > 0)
            {
                measure = Global.DialogFont.MeasureString(currentText);
                if (measure.X > maximumWidth)
                {
                    // Gets how many characters fit in the screen
                    int charsThatFit = (int)(currentText.Count() * maximumWidth / measure.X) - 1;

                    // Gets the index of the last space so we put a break there
                    int breakIndex = currentText.LastIndexOf(' ', charsThatFit, charsThatFit);

                    // Adds the break
                    string currentWithBreak = currentText.Insert(breakIndex, "\n");

                    // Adds this line to the finalString and remove it from the rest of the text that will be examined
                    finalString += currentWithBreak.Substring(0, currentWithBreak.IndexOf("\n") + 1);
                    currentText = currentWithBreak.Substring(currentWithBreak.IndexOf("\n") + 1).Trim();
                } else
                {
                    finalString += currentText;
                    break;
                }
            }

            // Iterates the words for height fit, if needed
            measure = Global.DialogFont.MeasureString(finalString);
            int cutIndex = 0;
            string originalString = finalString;
            char[] chars = new char[2] { ' ', '\n'};

            while (measure.Y > lineHeight * 5)
            {
                cutIndex = finalString.LastIndexOfAny(chars, finalString.Count() - 1, finalString.Count() - 1);

                finalString = finalString.Substring(0, cutIndex);
                measure = Global.DialogFont.MeasureString(finalString);
            }

            // Sets the final variables for use
            string remaining = originalString.Substring(cutIndex);
            if (remaining.Count() > 0 && remaining[0] == '\n')
                remaining = remaining.Substring(1);
            remainingText = remaining.Count() != finalString.Count() ? remaining : null;
            currentText = finalString;

            isAnimating = true;
        }

        // Singleton stuff
        private static DialogManager instance;
        protected DialogManager()
        {
            texture = Global.GetTexture("dialog_bg");
            bounds = Global.Viewport.Bounds;

            lineHeight = Global.DialogFont.LineSpacing;
            yOffset = lineHeight * 5 + marginY * 2;
        }
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
