using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Managers.Data;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StryfeRPG.System.Windows;
using StryfeRPG.Managers.GUI;
using StryfeCore.Models.Utils;

namespace StryfeRPG.Managers
{
    public class CharacterManager : WindowManager
    {
        private AttributeSheet Attributes;
        private Texture2D slotTexture;
        private int selectedAtt;

        public void AddExperience(int experience)
        {
            int oldLevel = Attributes.PureBase[CharacterAttribute.Level];

            Attributes.PureBase[CharacterAttribute.Experience] += experience;
            Attributes.Recalculate();

            QuickMessageManager.Instance.ShowMessage(new QuickMessage("Experience gained", string.Format("+{0}XP", experience)));

            int currentLevel = Attributes.PureBase[CharacterAttribute.Level];
            if (currentLevel > oldLevel)
                QuickMessageManager.Instance.ShowMessage(new QuickMessage("Level gained", string.Format("You're now level {0}!", currentLevel)));
        }

        public void AddModifier(AttributeModifier modifier)
        {
            AttributeSheet sheet = Global.Player.Attributes;
            switch (modifier.Type)
            {
                case ModifierType.Current:
                    int att = sheet.Current[modifier.Attribute] + modifier.Value;
                    if (att > sheet.Calculated[modifier.Attribute])
                        att = sheet.Calculated[modifier.Attribute];
                    else if (att < 0)
                        att = 0;
                    sheet.Current[modifier.Attribute] = att;
                    break;

                case ModifierType.Permanent:
                case ModifierType.Temporary:
                case ModifierType.Equipment:
                    sheet.Modifiers.Add(modifier);
                    break;
            }

            sheet.Recalculate();
        }

        public void RemoveModifiers(int itemId)
        {
            AttributeSheet sheet = Global.Player.Attributes;
            AttributeModifier mod = null;
            foreach (AttributeModifier att in sheet.Modifiers)
            {
                if (att.ItemId == itemId)
                {
                    mod = att;
                    break;
                }
            }

            if (mod != null)
            {
                sheet.Modifiers.Remove(mod);
                sheet.Recalculate();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            if (!IsOpened)
                return;

            WindowUtils.spriteBatch = spriteBatch;

            // Window
            Window window = new Window(bounds.Center.X - Width / 2, bounds.Center.Y - Height / 2, Width, Height);
            window.Margin = 20;

            // Attributes
            SpriteFont font = Global.DetailFont;
            int fontHeight = font.LineSpacing / 2;
            int innerMargin = 5;
            Vector2 maxSize = new Vector2(115, 50);
            int biggerX = (int)font.MeasureString("Intelligence:").X / 2;

            window.AddChild(new TextWindow(font, "Level:", new Vector2(biggerX - font.MeasureString("Level:").X / 2, 0), maxSize, Color.Cyan));

            window.AddChild(new TextWindow(font, "Vitality:", new Vector2(biggerX - font.MeasureString("Vitality:").X / 2, fontHeight + innerMargin), maxSize));
            window.AddChild(new TextWindow(font, "Wisdom:", new Vector2(biggerX - font.MeasureString("Wisdom:").X / 2, (fontHeight + innerMargin) * 2), maxSize));
            window.AddChild(new TextWindow(font, "Endurance:", new Vector2(biggerX - font.MeasureString("Endurance:").X / 2, (fontHeight + innerMargin) * 3), maxSize));
            window.AddChild(new TextWindow(font, "Strenght:", new Vector2(biggerX - font.MeasureString("Strenght:").X / 2, (fontHeight + innerMargin) * 4), maxSize));
            window.AddChild(new TextWindow(font, "Dexterity:", new Vector2(biggerX - font.MeasureString("Dexterity:").X / 2, (fontHeight + innerMargin) * 5), maxSize));
            window.AddChild(new TextWindow(font, "Intelligence:", new Vector2(0, (fontHeight + innerMargin) * 6), maxSize));
            window.AddChild(new TextWindow(font, "Faith:", new Vector2(biggerX - font.MeasureString("Faith:").X / 2, (fontHeight + innerMargin) * 7), maxSize));
            window.AddChild(new TextWindow(font, "Luck:", new Vector2(biggerX - font.MeasureString("Luck:").X / 2, (fontHeight + innerMargin) * 8), maxSize));

            window.AddChild(new TextWindow(font, "Points left:", new Vector2(biggerX - font.MeasureString("Points left:").X / 2, (fontHeight + innerMargin) * 9), maxSize, Color.Cyan));
            window.AddChild(new TextWindow(font, "Experience:", new Vector2(biggerX - font.MeasureString("Experience:").X / 2, (fontHeight + innerMargin) * 10), maxSize, Color.Cyan));

            // Values
            int x = biggerX + innerMargin;
            Dictionary<CharacterAttribute, int> atts = Attributes.PureBase;

            window.AddChild(new TextWindow(font, Attributes.PureBase[CharacterAttribute.Level].ToString(), new Vector2(x, 0), maxSize, Color.Cyan));

            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Vitality].ToString(), new Vector2(x, (fontHeight + innerMargin)), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Wisdom].ToString(), new Vector2(x, (fontHeight + innerMargin) * 2), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Endurance].ToString(), new Vector2(x, (fontHeight + innerMargin) * 3), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Strenght].ToString(), new Vector2(x, (fontHeight + innerMargin) * 4), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Dexterity].ToString(), new Vector2(x, (fontHeight + innerMargin) * 5), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Intelligence].ToString(), new Vector2(x, (fontHeight + innerMargin) * 6), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Faith].ToString(), new Vector2(x, (fontHeight + innerMargin) * 7), maxSize, Color.Yellow));
            window.AddChild(new TextWindow(font, atts[CharacterAttribute.Luck].ToString(), new Vector2(x, (fontHeight + innerMargin) * 8), maxSize, Color.Yellow));

            window.AddChild(new TextWindow(font, Attributes.PointsLeft.ToString(), new Vector2(x, (fontHeight + innerMargin) * 9), maxSize, Color.Cyan));
            window.AddChild(new TextWindow(font, Attributes.PureBase[CharacterAttribute.Experience].ToString(), new Vector2(x, (fontHeight + innerMargin) * 10), maxSize, Color.Cyan));

            // Buttons
            x += 100;
            int buttonSize = fontHeight * 2;
            innerMargin = 10;
            window.AddChild(new Window(x, (buttonSize + innerMargin), buttonSize, buttonSize, slotTexture, selectedAtt == 0 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 2, buttonSize, buttonSize, slotTexture, selectedAtt == 1 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 3, buttonSize, buttonSize, slotTexture, selectedAtt == 2 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 4, buttonSize, buttonSize, slotTexture, selectedAtt == 3 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 5, buttonSize, buttonSize, slotTexture, selectedAtt == 4 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 6, buttonSize, buttonSize, slotTexture, selectedAtt == 5 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 7, buttonSize, buttonSize, slotTexture, selectedAtt == 6 ? Color.Cyan : Color.White));
            window.AddChild(new Window(x, (buttonSize + innerMargin) * 8, buttonSize, buttonSize, slotTexture, selectedAtt == 7 ? Color.Cyan : Color.White));

            WindowUtils.DrawWindow(window);
        }

        public override void PerformAction()
        {
            if (Global.Player.Attributes.PointsLeft == 0)
                return;

            CharacterAttribute att = CharacterAttribute.Vitality;
            switch (selectedAtt)
            {
                case 0:
                    att = CharacterAttribute.Vitality;
                    break;
                case 1:
                    att = CharacterAttribute.Wisdom;
                    break;
                case 2:
                    att = CharacterAttribute.Endurance;
                    break;
                case 3:
                    att = CharacterAttribute.Strenght;
                    break;
                case 4:
                    att = CharacterAttribute.Dexterity;
                    break;
                case 5:
                    att = CharacterAttribute.Intelligence;
                    break;
                case 6:
                    att = CharacterAttribute.Faith;
                    break;
                case 7:
                    att = CharacterAttribute.Luck;
                    break;
            }

            Attributes.PureBase[att]++;
            Attributes.PointsLeft--;
            Attributes.Recalculate();
        }

        public override void Move(Vector2 movement)
        {
            selectedAtt += (int)movement.Y;
            if (selectedAtt < 0)
                selectedAtt = 0;
            else if (selectedAtt > 7)
                selectedAtt = 7;
        }

        // Singleton stuff
        private static CharacterManager instance;
        protected CharacterManager()
        {
            slotTexture = Global.GetTexture("item_bg");
            selectedAtt = 0;

            Width = 250;
            Height = 400;

            Attributes = Global.Player.Attributes;
        }
        public static CharacterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterManager();
                }
                return instance;
            }
        }
    }
}
