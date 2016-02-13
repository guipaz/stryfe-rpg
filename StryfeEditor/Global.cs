using StryfeRPG.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeEditor
{
    public static class Global
    {
        public static List<Item> Items = new List<Item>();
        public static int DefaultItemSize = 34;
        public static string ContentPath = "../../../StryfeRPG/Content";

        public static void SortItems()
        {
            Items = Items.OrderBy(o => o.Id).ToList();
        }
    }
}
