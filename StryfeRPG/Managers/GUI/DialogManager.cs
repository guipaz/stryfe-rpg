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
        private double cooldown = 0;
        private double charsBySecond = 60;
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
                    currentText = GetCurrentText(remainingText);
                    remainingText = null;
                    return;
                }

                // Finishes the dialog
                if (nextTextIndex >= currentDialog.Messages.Count())
                {
                    nextTextIndex = 0;
                    dialogActive = false;
                    currentDialog = null;
                    currentText = "";

                    if (ScriptInterpreter.Instance.IsScriptRunning())
                    {
                        ScriptInterpreter.Instance.FinishedCommand();
                    } else
                    {
                        if (dismissReference != null)
                            dismissReference.Dismiss();

                        dismissReference = null;
                    }
                } else // or shows the next message
                {
                    currentText = GetCurrentText(currentDialog.Messages[nextTextIndex]);
                    nextTextIndex++;
                }
            }
        }

        public void SkipMessage()
        {
            if (isAnimating)
            {
                isAnimating = false;
                return;
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
                }

                if (cooldown <= 0)
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
            Vector2 nameBoxSize = Vector2.Zero;
            if (currentDialog.CharacterName != null)
                nameBoxSize = new Vector2(Global.DialogFont.MeasureString(currentDialog.CharacterName).X + marginX * 2, Global.DialogFont.LineSpacing + marginY * 1.5f);

            spriteBatch.Draw(texture,
                             destinationRectangle: new Rectangle(0, bounds.Height - yOffset - (int)nameBoxSize.Y, (int)nameBoxSize.X, (int)nameBoxSize.Y),
                             color: new Color(Color.White, 0.8f));

            spriteBatch.Draw(texture,
                             destinationRectangle: new Rectangle(0, bounds.Height - yOffset, bounds.Width, yOffset),
                             color: new Color(Color.White, 0.8f));

            // Draw text
            if (currentDialog.CharacterName != null)
                Utils.DrawText(Global.DialogFont, spriteBatch, currentDialog.CharacterName, new Vector2(marginX, bounds.Height - yOffset + marginY - nameBoxSize.Y), Color.Yellow);
            Utils.DrawText(Global.DialogFont, spriteBatch, animatedText, new Vector2(marginX, bounds.Height - yOffset + marginY), Color.White);
        }

        private string GetCurrentText(string text)
        {
            // Calculates the text area
            float maximumWidth = Global.Viewport.Bounds.Width - (marginX * 2);

            string[] str = Utils.GetCroppedString(text, Global.DialogFont, maximumWidth, Global.DialogFont.LineSpacing * 5);
            remainingText = str[1];

            isAnimating = true;
            return str[0];
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
