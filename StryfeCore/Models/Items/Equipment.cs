using StryfeRPG.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Models.Items
{
    public enum EquipmentType
    {
        Armor,
        Weapon,
        Shield,
        Helmet,
        Cape,
        Shoes,
        Mask,
        Accessory
    }

    public class Equipment : Item
    {
        public EquipmentType EquipType { get; set; }
    }
}
