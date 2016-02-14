using StryfeRPG.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public enum CharacterAttribute
    {
        Level,
        Experience,

        // Base
        Vitality, // Max HP
        Wisdom, // Max MP
        Endurance, // Max Stamina, Equip burden, Defense
        Strenght, // Equip requirements, physical damage
        Dexterity, // Equip requirements, physical damage, stamina
        Intelligence, // Sorcery requirements, magical damage, Max MP
        Faith, // Miracle requirements, magical damage, Max MP
        Luck, // Critical, luck rolls

        // Calculated
        HP,
        MP,
        Stamina,
        PhysicalDamage,
        MagicalDamage,
        PhysicalDefense,
        MagicalDefense,
        CriticalChance,
    }

    public class AttributeSheet
    {
        public Dictionary<CharacterAttribute, int> PureBase = new Dictionary<CharacterAttribute, int>(); // without modifiers
        public Dictionary<CharacterAttribute, int> Base = new Dictionary<CharacterAttribute, int>();
        public Dictionary<CharacterAttribute, int> Calculated = new Dictionary<CharacterAttribute, int>();
        public Dictionary<CharacterAttribute, int> Current = new Dictionary<CharacterAttribute, int>();

        // Item bonuses, equipment modifiers and such
        public List<AttributeModifier> Modifiers = new List<AttributeModifier>();

        public AttributeSheet()
        {
            PureBase[CharacterAttribute.Level] = 1;
            PureBase[CharacterAttribute.Experience] = 0;

            PureBase[CharacterAttribute.Vitality] = 1;
            PureBase[CharacterAttribute.Wisdom] = 1;
            PureBase[CharacterAttribute.Endurance] = 1;
            PureBase[CharacterAttribute.Strenght] = 1;
            PureBase[CharacterAttribute.Dexterity] = 1;
            PureBase[CharacterAttribute.Intelligence] = 1;
            PureBase[CharacterAttribute.Faith] = 1;
            PureBase[CharacterAttribute.Luck] = 1;
            
            Recalculate();
            ResetCurrent();
        }

        public void Recalculate()
        {
            // Creates a temporary dictionary
            Dictionary<CharacterAttribute, int> TempBase = new Dictionary<CharacterAttribute, int>();
            foreach (KeyValuePair<CharacterAttribute, int> pair in PureBase)
                TempBase.Add(pair.Key, pair.Value);

            // Applies the base modifiers
            foreach (AttributeModifier mod in Modifiers)
            {
                if (TempBase.ContainsKey(mod.Attribute))
                    TempBase[mod.Attribute] += mod.Value;
            }

            Base = TempBase;

            // Some variables for proportional current stats
            Dictionary<CharacterAttribute, float> CurrentProportion = new Dictionary<CharacterAttribute, float>();
            foreach (KeyValuePair<CharacterAttribute, int> att in Current)
            {
                CurrentProportion.Add(att.Key, Current[att.Key] / Calculated[att.Key]);
            }
            
            // Calculates everything
            if (TempBase[CharacterAttribute.Experience] >= TempBase[CharacterAttribute.Level] * 100)
            {
                TempBase[CharacterAttribute.Level]++;
                TempBase[CharacterAttribute.Experience] = 0;
                ResetCurrent();
            }
            
            Calculated[CharacterAttribute.HP] = TempBase[CharacterAttribute.Vitality] * 3 + TempBase[CharacterAttribute.Strenght];
            Calculated[CharacterAttribute.MP] = TempBase[CharacterAttribute.Wisdom] * 3 + TempBase[CharacterAttribute.Intelligence] + TempBase[CharacterAttribute.Faith];
            Calculated[CharacterAttribute.Stamina] = TempBase[CharacterAttribute.Endurance] * 3 + TempBase[CharacterAttribute.Dexterity];
            Calculated[CharacterAttribute.PhysicalDamage] = TempBase[CharacterAttribute.Strenght] * 3 + TempBase[CharacterAttribute.Dexterity] * 3;
            Calculated[CharacterAttribute.MagicalDamage] = TempBase[CharacterAttribute.Intelligence] * 3 + TempBase[CharacterAttribute.Faith] * 3;
            Calculated[CharacterAttribute.CriticalChance] = (int)(TempBase[CharacterAttribute.Luck] * 1.5f);
            Calculated[CharacterAttribute.PhysicalDefense] = TempBase[CharacterAttribute.Endurance] * 3 + TempBase[CharacterAttribute.Strenght] + TempBase[CharacterAttribute.Dexterity];
            Calculated[CharacterAttribute.MagicalDefense] = TempBase[CharacterAttribute.Endurance] * 2 + TempBase[CharacterAttribute.Intelligence] + TempBase[CharacterAttribute.Faith];

            // Applies the calculated modifiers
            foreach (AttributeModifier mod in Modifiers)
            {
                if (Calculated.ContainsKey(mod.Attribute))
                    Calculated[mod.Attribute] += mod.Value;
            }

            // Define current stats (HP, MP and Stamina are proportional)
            if (CurrentProportion.Count() > 0)
            {
                Current[CharacterAttribute.HP] = (int)(Calculated[CharacterAttribute.HP] * CurrentProportion[CharacterAttribute.HP]);
                Current[CharacterAttribute.MP] = (int)(Calculated[CharacterAttribute.MP] * CurrentProportion[CharacterAttribute.MP]);
                Current[CharacterAttribute.Stamina] = (int)(Calculated[CharacterAttribute.Stamina] * CurrentProportion[CharacterAttribute.Stamina]);
                Current[CharacterAttribute.PhysicalDamage] = (int)(Calculated[CharacterAttribute.PhysicalDamage] * CurrentProportion[CharacterAttribute.PhysicalDamage]);
                Current[CharacterAttribute.MagicalDamage] = (int)(Calculated[CharacterAttribute.MagicalDamage] * CurrentProportion[CharacterAttribute.MagicalDamage]);
                Current[CharacterAttribute.CriticalChance] = (int)(Calculated[CharacterAttribute.CriticalChance] * CurrentProportion[CharacterAttribute.CriticalChance]);
                Current[CharacterAttribute.PhysicalDefense] = (int)(Calculated[CharacterAttribute.PhysicalDefense] * CurrentProportion[CharacterAttribute.PhysicalDefense]);
                Current[CharacterAttribute.MagicalDefense] = (int)(Calculated[CharacterAttribute.MagicalDefense] * CurrentProportion[CharacterAttribute.MagicalDefense]);
            }
        }

        public void ResetCurrent()
        {
            Current[CharacterAttribute.HP] = Calculated[CharacterAttribute.HP];
            Current[CharacterAttribute.MP] = Calculated[CharacterAttribute.MP];
            Current[CharacterAttribute.Stamina] = Calculated[CharacterAttribute.Stamina];
            Current[CharacterAttribute.PhysicalDamage] = Calculated[CharacterAttribute.PhysicalDamage];
            Current[CharacterAttribute.MagicalDamage] = Calculated[CharacterAttribute.MagicalDamage];
            Current[CharacterAttribute.CriticalChance] = Calculated[CharacterAttribute.CriticalChance];
            Current[CharacterAttribute.PhysicalDefense] = Calculated[CharacterAttribute.PhysicalDefense];
            Current[CharacterAttribute.MagicalDefense] = Calculated[CharacterAttribute.MagicalDefense];
        }
    }
}
