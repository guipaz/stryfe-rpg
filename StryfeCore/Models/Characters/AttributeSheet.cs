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
        Vitality, // Max HP
        Wisdom, // Max MP
        Endurance, // Max Stamina, Equip burden, Defense
        Strenght, // Equip requirements, physical damage
        Dexterity, // Equip requirements, physical damage, stamina
        Intelligence, // Sorcery requirements, magical damage, Max MP
        Faith, // Miracle requirements, magical damage, Max MP
        Luck, // Critical, luck rolls
        HP,
        MP,
        Stamina,
        PhysicalDamage,
        MagicalDamage,
        CriticalChance,
        PhysicalDefense,
        MagicalDefense
    }

    public class AttributeSheet
    {
        public Dictionary<CharacterAttribute, int> Base = new Dictionary<CharacterAttribute, int>();
        public Dictionary<CharacterAttribute, int> Calculated = new Dictionary<CharacterAttribute, int>();
        public Dictionary<CharacterAttribute, int> Current = new Dictionary<CharacterAttribute, int>();

        // Item bonuses, equipment modifiers and such
        public List<AttributeModifier> Modifiers = new List<AttributeModifier>();

        public AttributeSheet()
        {
            Base[CharacterAttribute.Level] = 1;
            Base[CharacterAttribute.Experience] = 0;

            Base[CharacterAttribute.Vitality] = 1;
            Base[CharacterAttribute.Wisdom] = 1;
            Base[CharacterAttribute.Endurance] = 1;
            Base[CharacterAttribute.Strenght] = 1;
            Base[CharacterAttribute.Dexterity] = 1;
            Base[CharacterAttribute.Intelligence] = 1;
            Base[CharacterAttribute.Faith] = 1;
            Base[CharacterAttribute.Luck] = 1;

            Recalculate();
            ResetCurrent();
        }

        public void Recalculate()
        {
            if (Base[CharacterAttribute.Experience] >= Base[CharacterAttribute.Level] * 100)
            {
                Base[CharacterAttribute.Level]++;
                Base[CharacterAttribute.Experience] = 0;
            }

            Calculated[CharacterAttribute.HP] = Base[CharacterAttribute.Vitality] * 3 + Base[CharacterAttribute.Strenght];
            Calculated[CharacterAttribute.MP] = Base[CharacterAttribute.Wisdom] * 3 + Base[CharacterAttribute.Intelligence] + Base[CharacterAttribute.Faith];
            Calculated[CharacterAttribute.Stamina] = Base[CharacterAttribute.Endurance] * 3 + Base[CharacterAttribute.Dexterity];
            Calculated[CharacterAttribute.PhysicalDamage] = Base[CharacterAttribute.Strenght] * 3 + Base[CharacterAttribute.Dexterity] * 3;
            Calculated[CharacterAttribute.MagicalDamage] = Base[CharacterAttribute.Intelligence] * 3 + Base[CharacterAttribute.Faith] * 3;
            Calculated[CharacterAttribute.CriticalChance] = (int)(Base[CharacterAttribute.Luck] * 1.5f);
            Calculated[CharacterAttribute.PhysicalDefense] = Base[CharacterAttribute.Endurance] * 3 + Base[CharacterAttribute.Strenght] + Base[CharacterAttribute.Dexterity];
            Calculated[CharacterAttribute.MagicalDefense] = Base[CharacterAttribute.Endurance] * 2 + Base[CharacterAttribute.Intelligence] + Base[CharacterAttribute.Faith];
        }

        public void ResetCurrent()
        {
            Current[CharacterAttribute.HP] = Calculated[CharacterAttribute.HP];
            Current[CharacterAttribute.MP] = Calculated[CharacterAttribute.MP];
            Current[CharacterAttribute.Stamina] = Calculated[CharacterAttribute.Stamina];
            Current[CharacterAttribute.PhysicalDamage] = Calculated[CharacterAttribute.PhysicalDamage];
            Current[CharacterAttribute.MagicalDamage] = Calculated[CharacterAttribute.MagicalDamage];
            Current[CharacterAttribute.CriticalChance] = Calculated[CharacterAttribute.CriticalChance];
        }
    }
}
