using StryfeRPG.Managers;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace StryfeRPG.Models.Maps.Objects
{
    public class ItemObject : MapObject
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public ItemObject(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            if (ScriptId == -1)
            {
                Item = Global.GetItem(int.Parse(obj.Properties["item"]));
                Quantity = obj.Properties.ContainsKey("quantity") ? int.Parse(obj.Properties["quantity"]) : 1;
            }
        }

        public override void PerformAction()
        {
            base.PerformAction();

            if (ScriptId == -1)
            {
                InventoryManager.Instance.AddItem(Item, Quantity);
            }

            SavedInformation.IsActive = false;
        }
    }
}
