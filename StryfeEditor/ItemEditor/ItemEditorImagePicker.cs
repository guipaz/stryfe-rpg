using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StryfeEditor.ItemEditor
{
    public partial class ItemEditorImagePicker : Form
    {
        public string imageName { get; set; }
        public int gid { get; set; }

        public int tileSize { get; private set; }
        public List<ItemEditorImagePickerListener> listeners { get; set; }

        private Image image;
        private int tilesWide;
        
        public ItemEditorImagePicker()
        {
            InitializeComponent();
            tileSize = Global.DefaultItemSize;
            listeners = new List<ItemEditorImagePickerListener>();
        }

        public void SetTileSize(int size)
        {
            tileSize = size;

            if (image == null)
                return;
            
            int width = image.Width + pictureBox1.Bounds.X + 20;
            if (tileSize == 0)
                tileSize = Global.DefaultItemSize;

            tilesWide = width / tileSize;

            Console.WriteLine("{0} - {1}", imageName, tilesWide);
        }

        public void SelectGid(int gid)
        {
            int x = gid % tilesWide;
            int y = gid / tilesWide;

            SelectTile(x, y);
        }

        public void SetImage(string path)
        {
            int slashIndex = path.LastIndexOf("\\") + 1;
            if (slashIndex == 0)
                slashIndex = path.LastIndexOf("/") + 1;
            string currentImage = path.Substring(slashIndex, path.Count() - slashIndex);
            currentImage = currentImage.Substring(0, currentImage.IndexOf('.'));
            if (currentImage == imageName)
                return;

            imageName = currentImage;

            image = Image.FromFile(path);

            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            
            int width = image.Width + pictureBox1.Bounds.X + 20;
            int height = image.Height + pictureBox1.Bounds.Y + 20;

            if (tileSize == 0)
                tileSize = Global.DefaultItemSize;

            tilesWide = width / tileSize;

            Console.WriteLine("{0} - {1}", path, tilesWide);

            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(width, 500);

            this.AutoScroll = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X / tileSize;
            int y = e.Location.Y / tileSize;

            SelectTile(x, y);
        }

        public void ResetGid()
        {
            gid = -1;
            foreach (ItemEditorImagePickerListener listener in listeners)
                listener.SelectedTile(null, gid);
        }

        private void SelectTile(int x, int y)
        {
            gid = y * tilesWide + x;

            Bitmap original = new Bitmap(image);
            Rectangle srcRect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
            Bitmap cropped = original.Clone(srcRect, original.PixelFormat);

            foreach (ItemEditorImagePickerListener listener in listeners)
                listener.SelectedTile(cropped, gid);
        }
    }
}
