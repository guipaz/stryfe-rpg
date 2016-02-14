using StryfeCore.Models.Items;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers.Data
{
    public class EquipmentManager
    {
        public List<Equipment> Equipped = new List<Equipment>();

        public void ToggleEquipment(Item item)
        {
            Equipment equip = (Equipment)item;

            Item removeItem = null;
            bool sameItem = false;
            int accessories = 0; // can equip 2 accessories

            foreach (Equipment e in Equipped)
            {
                if (e.EquipType == equip.EquipType)
                {
                    if (e.Id == equip.Id)
                        sameItem = true;

                    if (e.EquipType == EquipmentType.Accessory)
                    {
                        accessories++;
                        if (accessories < 2 && !sameItem)
                            continue;
                    }

                    removeItem = e;
                    Equipped.Remove(e);

                    break;
                }
            }
            
            if (removeItem != null)
            {
                CharacterManager.Instance.RemoveModifiers(removeItem.Id);
            }

            if (sameItem)
            {
                Utils.PrintStats();
                return;
            }
                

            foreach (AttributeModifier mod in equip.Modifiers)
                CharacterManager.Instance.AddModifier(mod);

            Equipped.Add(equip);

            Utils.PrintStats();
        }

        public bool IsItemEquipped(int id)
        {
            foreach (Equipment e in Equipped)
                if (e.Id == id)
                    return true;
            return false;
        }

        // Singleton stuff
        private static EquipmentManager instance;
        protected EquipmentManager() { }
        public static EquipmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EquipmentManager();
                }
                return instance;
            }
        }
    }
}
