using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        // Data management
        private Dictionary<int, Item> Inventory = new Dictionary<int, Item>(); // Key: position - Value: item
        private Dictionary<int, int> Quantities = new Dictionary<int, int>(); // Key: item id - Value: quantity
        private Item selectedItem;
        private Vector2 selectedItemIndex;

        // Screen management
        public bool IsOpened { get; private set; }
        private int Width = 550;
        private int Height = 300;

        private int tilesX = 4;
        private int tilesY = 4;

        private Texture2D bgTexture;
        private Texture2D itemTexture;
        private Rectangle bounds;

        public void ToggleInventory()
        {
            if (IsOpened)
                CloseInventory();
            else
                OpenInventory();
        }

        public void OpenInventory()
        {
            IsOpened = true;
            CheckSelectedItem();
        }

        public void CloseInventory()
        {
            IsOpened = false;
        }

        public void AddItem(int id, int quantity)
        {
            Item item = Global.GetItem(id);
            if (item != null)
                AddItem(item, quantity);
        }

        public void AddItem(Item item, int quantity)
        {
            if (item != null)
            {
                Inventory[GetAvailablePosition()] = item;
                Quantities[item.Id] = quantity;
            }
        }

        private int GetAvailablePosition()
        {
            List<int> positions = Inventory.Keys.ToList();
            positions.Sort();
            
            int pos = 0;
            foreach (int occupiedPos in positions)
            {
                if (pos < occupiedPos)
                    return pos;
                pos++;
            }

            return pos;
        }

        public void UseItem(Item item)
        {

        }

        public void Move(Vector2 movement)
        {
            Vector2 finalPos = new Vector2(selectedItemIndex.X + movement.X, selectedItemIndex.Y + movement.Y);
            if (finalPos.X >= 0 &&
                finalPos.Y >= 0 &&
                finalPos.X < tilesX &&
                finalPos.Y < tilesY)
                selectedItemIndex = finalPos;

            CheckSelectedItem();
        }

        private void CheckSelectedItem()
        {
            int i = (int)selectedItemIndex.Y * tilesX + (int)selectedItemIndex.X;
            if (Inventory.ContainsKey(i))
                selectedItem = Inventory[i];
            else
                selectedItem = null;
        }

        public void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            if (!IsOpened)
                return;

            // Window
            int windowX = bounds.Width / 2 - Width / 2;
            spriteBatch.Draw(bgTexture,
                             destinationRectangle: new Rectangle(windowX, bounds.Height / 2 - Height / 2, Width, Height),
                             color: new Color(Color.White, 0.8f));

            // Item slots
            int margin = 20;
            int itemX = bounds.Width / 2 - Width / 2 + margin;
            int itemY = bounds.Height / 2 - Height / 2 + margin;
            int itemSize = 50;
            int lastX = 0;
            
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    lastX = itemX + (x * (itemSize + margin));

                    Color color = Color.White;
                    int i = (int)y * tilesX + (int)x;
                    if (Inventory.ContainsKey(i) && Inventory[i] != null)
                        color = Color.LightBlue; // when there's an item in the slot

                    if (selectedItemIndex.X == x && selectedItemIndex.Y == y)
                        color = Color.Cyan; // when the slot is selected
                    
                    spriteBatch.Draw(itemTexture,
                                 destinationRectangle: new Rectangle(lastX, itemY + y * (itemSize + margin), itemSize, itemSize),
                                 color: color);
                }
            }

            // Description frame
            itemX = lastX + itemSize + margin;
            spriteBatch.Draw(itemTexture,
                             destinationRectangle: new Rectangle(itemX, itemY, windowX + Width - itemX - margin, Height - margin * 2),
                             color: Color.White);

            //TODO: item sprite

            // Item name
            if (selectedItem != null)
            {
                string str = Utils.GetCroppedString(selectedItem.Name, Global.DialogFont, windowX + Width - itemX - (margin * 3), Height - margin * 4)[0];
                spriteBatch.DrawString(Global.DialogFont, str, new Vector2(itemX + margin, itemY + margin), Color.White);
            }

            //TODO: item description
        }

        // Singleton stuff
        private static InventoryManager instance;
        protected InventoryManager()
        {
            bgTexture = Global.GetTexture("dialog_bg");
            itemTexture = Global.GetTexture("item_bg");
            bounds = Global.Viewport.Bounds;

            Inventory = new Dictionary<int, Item>();
            Quantities = new Dictionary<int, int>();
        }

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
