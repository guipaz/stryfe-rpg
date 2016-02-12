using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers
{
    public class InventoryManager
    {
        public void AddItem(int id, int quantity)
        {
            Item item = Global.GetItem(id);
            if (item != null)
                AddItem(item, quantity);
        }

        public void AddItem(Item item, int quantity)
        {
            Global.Player.Inventory.Add(item); //TODO quantity
        }

        // Singleton stuff
        private static InventoryManager instance;
        protected InventoryManager() { }
        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventoryManager();
                }
                return instance;
            }
        }
    }
}
