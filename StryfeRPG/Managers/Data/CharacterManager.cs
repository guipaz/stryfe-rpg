using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers
{
    public class CharacterManager
    {
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

        // Singleton stuff
        private static CharacterManager instance;
        protected CharacterManager() { }
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
