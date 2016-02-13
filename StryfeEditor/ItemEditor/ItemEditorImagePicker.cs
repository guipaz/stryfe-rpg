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
    public partial class ItemEditorImagePicker : Form
    {
        public Image selectedTileImage { get; set; }
        public int selectedTileIndex { get; set; }
        public int tileSize { get; set; }
        public List<ItemEditorImagePickerListener> listeners { get; set; }

        private Image image;
        private int tilesWide;

        public ItemEditorImagePicker()
        {
            InitializeComponent();
            tileSize = 34;
            SelectImage("D:/Dev/stryfe-rpg/StryfeRPG/Content/Textures/items1.png");
            listeners = new List<ItemEditorImagePickerListener>();
        }

        public void SelectImage(string path)
        {
            image = Image.FromFile(path);

            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            
            int width = image.Width + pictureBox1.Bounds.X + 20;
            int height = image.Height + pictureBox1.Bounds.Y + 20;

            tilesWide = width / tileSize;

            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(width, 500);

            this.AutoScroll = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X / tileSize;
            int y = e.Location.Y / tileSize;

            selectedTileIndex = y * tilesWide + x;

            Bitmap original = new Bitmap(image);
            Rectangle srcRect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
            Bitmap cropped = original.Clone(srcRect, original.PixelFormat);

            foreach (ItemEditorImagePickerListener listener in listeners)
                listener.SelectedTile(cropped, selectedTileIndex);
        }
    }
}
