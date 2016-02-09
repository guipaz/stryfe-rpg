using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Characters
{
    public class AttributeSheet
    {
        // Level information
        public int Level = 1;
        public int Experience = 0;

        // Base attributes
        public int Vitality = 1; // Max HP
        public int Wisdom = 1; // Max MP
        public int Endurance = 1; // Max Stamina, Equip burden
        public int Strenght = 1; // Equip requirements, physical damage
        public int Dexterity = 1; // Equip requirements, physical damage, stamina
        public int Intelligence = 1; // Sorcery requirements, magical damage, Max MP
        public int Faith = 1; // Miracle requirements, magical damage, Max MP
        public int Luck = 1; // Critical, luck rolls

        // Calculated attributes
        public int MaxHP;
        public int MaxMP;
        public int MaxStamina;
        public int PhysicalDamage;
        public int MagicalDamage;
        public int CriticalChance;

        // Current Attributes
        public int CurrentHP;
        public int CurrentMP;
        public int CurrentStamina;
        public int CurrentPhysicalDamage;
        public int CurrentMagicalDamage;
        public int CurrentCriticalChance;
        
        public void Recalculate()
        {
            if (Experience >= Level * 100)
            {
                Level++;
                Experience = 0;
            }

            MaxHP = Vitality * 3 + Strenght;
            MaxMP = Wisdom * 3 + Intelligence + Faith;
            MaxStamina = Endurance * 3 + Dexterity;
            PhysicalDamage = Strenght * 3 + Dexterity * 3;
            MagicalDamage = Intelligence * 3 + Faith * 3;
            CriticalChance = (int)(Luck * 1.5f);
        }

        public void ResetCurrent()
        {
            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
            CurrentStamina = MaxStamina;
            CurrentPhysicalDamage = PhysicalDamage;
            CurrentMagicalDamage = MagicalDamage;
            CurrentCriticalChance = CriticalChance;
        }
    }
}
