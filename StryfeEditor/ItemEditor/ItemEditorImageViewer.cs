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
    public partial class ItemEditorImageViewer : Form, ItemEditorImagePickerListener
    {
        public ItemEditorImageViewer()
        {
            InitializeComponent();
        }

        public void SelectedTile(Image img, int gid)
        {
            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            this.Size = new Size(img.Width, img.Height >= 100 ? img.Height : 100);
        }
    }
}
