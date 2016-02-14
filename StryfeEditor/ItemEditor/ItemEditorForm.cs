using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StryfeRPG.Models.Items;
using StryfeRPG.Models.Characters;
using System.Globalization;
using StryfeCore.Models.Items;

namespace StryfeEditor.ItemEditor
{
    public partial class ItemEditorForm : Form, ItemEditorImagePickerListener
    {
        ItemEditorImagePicker imagePicker;
        ItemEditorImageViewer imageViewer;

        Item currentItem;
        AttributeModifier currentModifier;

        List<AttributeModifier> modifiers = new List<AttributeModifier>();

        public ItemEditorForm()
        {
            InitializeComponent();

            FillCombos();

            Utils.LoadItems();
            PopulateList();
            
        }

        public void FillCombos()
        {
            type.Items.Clear();
            equipType.Items.Clear();
            modAttribute.Items.Clear();
            modType.Items.Clear();

            // Type
            var values = Enum.GetValues(typeof(ItemType));
            foreach (object o in values)
                type.Items.Add(o.ToString());

            // EquipType
            values = Enum.GetValues(typeof(EquipmentType));
            foreach (object o in values)
                equipType.Items.Add(o.ToString());

            // Attributes
            values = Enum.GetValues(typeof(CharacterAttribute));
            foreach (object o in values)
                modAttribute.Items.Add(o.ToString());

            // Modifier Type
            values = Enum.GetValues(typeof(ModifierType));
            foreach (object o in values)
                modType.Items.Add(o.ToString());
        }

        private void PopulateList()
        {
            Global.SortItems();

            itemList.Items.Clear();
            foreach (Item i in Global.Items)
            {
                itemList.Items.Add(i);
            }
        }

        public void SelectedTile(Image img, int gid)
        {
            gidLabel.Text = String.Format("gid: {0}", gid);
        }

        public void ShowImagePicker()
        {
            imagePicker = new ItemEditorImagePicker();
            imagePicker.listeners.Add(this);
            imagePicker.Top = Top;
            imagePicker.Left = Left + Width;
            imagePicker.Show();        
            
            imageViewer = new ItemEditorImageViewer();
            imageViewer.Top = Top;
            imageViewer.Left = Left - imageViewer.Width;
            imageViewer.Show();

            Focus();

            imagePicker.listeners.Add(imageViewer);
        }

        private void ItemEditorForm_Shown(object sender, EventArgs e)
        {
            ShowImagePicker();
        }

        private void ItemEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            imagePicker.Close();
            imageViewer.Close();

            Utils.SaveItems();
        }

        private void Clear()
        {
            currentItem = null;
            currentModifier = null;
            imagePicker.ResetGid();

            var allTextBoxes = Utils.GetChildControls<TextBox>(this);
            foreach (TextBox tb in allTextBoxes)
            {
                tb.Text = "";
            }

            ClearModifier();
            modList.Items.Clear();
            modifiers.Clear();
        }

        private void ClearModifier()
        {
            modAttribute.Text = "";
            modType.Text = "";
            modValue.Text = "";
        }

        private void changeTextureButton_Click(object sender, EventArgs e)
        {
            string filename = "";

            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                filename = ofd.FileName;
            }

            if (filename != "")
            {
                imagePicker.SetImage(filename);
            }
        }
        
        private void tileSize_TextChanged(object sender, EventArgs e)
        {
            int size = Global.DefaultItemSize;
            int.TryParse(tileSize.Text, out size);
            imagePicker.SetTileSize(size > 0 ? size : Global.DefaultItemSize);
        }

        private void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemList.SelectedIndex >= 0 && itemList.SelectedIndex < Global.Items.Count)
            {
                Clear();
                currentItem = Global.Items[itemList.SelectedIndex];
                FillItem();
            }
        }

        private void FillItem()
        {
            if (currentItem == null)
                return;
            
            name.Text = currentItem.Name;
            type.Text = currentItem.Type.ToString();
            if (currentItem.Type == ItemType.Equipment)
                equipType.Text = ((Equipment)currentItem).EquipType.ToString();
            else
                equipType.Text = "";
            price.Text = String.Format("{0}", currentItem.Price);
            description.Text = currentItem.Description;
            script.Text = currentItem.ScriptId.ToString();
            tileSize.Text = currentItem.TextureTileSize.ToString();
            if (currentItem.TextureName != null && currentItem.TextureName.Count() > 0)
            {
                imagePicker.SetImage(String.Format("{0}/Textures/{1}.png", Global.ContentPath, currentItem.TextureName));
                imagePicker.SetTileSize(currentItem.TextureTileSize);
                if (currentItem.Gid != -1)
                {
                    imagePicker.SelectGid(currentItem.Gid);
                }
            }

            modifiers = (List<AttributeModifier>)currentItem.Modifiers.Clone();
            LoadModifiers();
        }

        private void LoadModifiers()
        {
            modList.Items.Clear();
            
            foreach (AttributeModifier mod in modifiers)
            {
                modList.Items.Add(mod);
            }
        }

        private void FillModifier()
        {
            modAttribute.Text = currentModifier.Attribute.ToString();
            modType.Text = currentModifier.Type.ToString();
            modValue.Text = currentModifier.Value.ToString();
        }

        private void modNewButton_Click(object sender, EventArgs e)
        {
            currentModifier = null;
            modList.SelectedIndex = -1;
            ClearModifier();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            Clear();
            itemList.SelectedIndex = -1;
        }

        private void modList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentItem != null && modList.SelectedIndex >= 0 && modList.SelectedIndex < currentItem.Modifiers.Count)
            {
                currentModifier = modifiers[modList.SelectedIndex];
                FillModifier();
            }
        }

        private void modEditButton_Click(object sender, EventArgs e)
        {
            if (currentModifier != null)
                modifiers.Remove(currentModifier);
            else
                currentModifier = new AttributeModifier();
            
            currentModifier.Attribute = (CharacterAttribute)Enum.Parse(typeof(CharacterAttribute), modAttribute.Text);
            currentModifier.Type = (ModifierType)Enum.Parse(typeof(ModifierType), modType.Text);
            currentModifier.Value = int.Parse(modValue.Text);

            modifiers.Add(currentModifier);

            LoadModifiers();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int pos = -1;
            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), Utils.GetTitleCase(type.Text));
            if (currentItem == null)
            {
                if (itemType == ItemType.Equipment)
                    currentItem = new Equipment();
                else
                    currentItem = new Item();

                currentItem.Id = GetNextId();
            } else
            {
                pos = Global.Items.IndexOf(currentItem);
            }

            currentItem.Name = name.Text;
            currentItem.Type = itemType;

            if (itemType == ItemType.Equipment)
                ((Equipment)currentItem).EquipType = (EquipmentType)Enum.Parse(typeof(EquipmentType), Utils.GetTitleCase(equipType.Text));

            currentItem.Description = description.Text;
            currentItem.ScriptId = script.Text.Count() > 0 ? int.Parse(script.Text) : -1;

            if (price.Text != null && price.Text.Count() > 0)
                currentItem.Price = int.Parse(price.Text);

            if (imagePicker.imageName != null && imagePicker.gid != -1)
            {
                currentItem.TextureName = imagePicker.imageName;
                currentItem.TextureTileSize = tileSize.Text.Count() > 0 ? int.Parse(tileSize.Text) : Global.DefaultItemSize;
                currentItem.Gid = imagePicker.gid;
            }

            foreach (AttributeModifier mod in modifiers)
                mod.ItemId = currentItem.Id;

            currentItem.Modifiers = (List<AttributeModifier>)modifiers.Clone();

            if (pos > -1)
                Global.Items[pos] = currentItem;
            else
                Global.Items.Add(currentItem);

            PopulateList();
        }

        private int GetNextId()
        {
            List<int> ids = new List<int>();
            foreach (Item i in Global.Items)
                ids.Add(i.Id);
            ids.Sort();

            int newId = 0;
            foreach (int id in ids)
            {
                if (newId < id)
                    return newId;
                newId++;
            }

            return newId;
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (currentItem != null)
            {
                Global.Items.Remove(currentItem);
                PopulateList();
                Clear();
            }
        }

        private void modDelButton_Click(object sender, EventArgs e)
        {
            if (currentModifier != null)
            {
                modifiers.Remove(currentModifier);
                LoadModifiers();
                ClearModifier();
            }
        }
    }
}
