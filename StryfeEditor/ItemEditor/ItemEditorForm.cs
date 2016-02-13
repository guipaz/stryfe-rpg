using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StryfeEditor.ItemEditor
{
    public partial class ItemEditorForm : Form, ItemEditorImagePickerListener
    {
        ItemEditorImagePicker imagePicker;
        ItemEditorImageViewer imageViewer;

        public ItemEditorForm()
        {
            InitializeComponent();
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

        private void button4_Click(object sender, EventArgs e)
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
                imagePicker.SelectImage(filename);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            int size = 32;
            int.TryParse(textBox7.Text, out size);
            imagePicker.tileSize = size > 0 ? size : 32;
        }

        private void ItemEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            imagePicker.Close();
            imageViewer.Close();
        }
    }
}
