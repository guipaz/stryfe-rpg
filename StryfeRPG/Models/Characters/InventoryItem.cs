using StryfeRPG.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Models.Characters
{
    public class InventoryItem
    {
        public int InventoryId { get; set; }
        public Item Item { get; set; }
    }
}
