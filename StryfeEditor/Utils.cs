using Newtonsoft.Json;
using StryfeRPG.Models.Items;
using StryfeRPG.System.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StryfeEditor
{
    public static class Utils
    {
        public static IEnumerable<TControl> GetChildControls<TControl>(Control control) where TControl : Control
        {
            var children = (control.Controls != null) ? control.Controls.OfType<TControl>() : Enumerable.Empty<TControl>();
            return children.SelectMany(c => GetChildControls<TControl>(c)).Concat(children);
        }

        public static void LoadItems()
        {
            List<Item> itemsJson = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(String.Format("{0}/Data/items.json", Global.ContentPath)), new ItemConverter());

            List<Item> items = new List<Item>();
            foreach (Item i in itemsJson)
                items.Add(i);

            Global.Items = items;
        }

        public static string GetTitleCase(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static void SaveItems()
        {
            string items = JsonConvert.SerializeObject(Global.Items);
            File.WriteAllText(String.Format("{0}/Data/items.json", Global.ContentPath), items);
        }
    }
}
