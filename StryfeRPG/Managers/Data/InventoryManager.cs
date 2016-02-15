using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeRPG.Managers.Data;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers
{
    public class InventoryManager : WindowManager
    {
        // Data management
        private Dictionary<int, Item> Inventory = new Dictionary<int, Item>(); // Key: position - Value: item
        private Dictionary<int, int> Quantities = new Dictionary<int, int>(); // Key: item id - Value: quantity

        private Item selectedItem;
        private Vector2 selectedItemIndex;

        // Screen management
        private int tilesX = 4;
        private int tilesY = 4;
        
        private Texture2D slotTexture;

        public override void OpenWindow()
        {
            base.OpenWindow();
            CheckSelectedItem();
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
                Inventory[GetAvailablePosition(item)] = item;
                Quantities[item.Id] = Quantities.ContainsKey(item.Id) ? Quantities[item.Id] + quantity : quantity;
            }

            ScriptInterpreter.Instance.FinishedCommand();
        }

        private int GetAvailablePosition(Item item)
        {
            List<int> positions = Inventory.Keys.ToList();
            positions.Sort();
            
            int pos = 0;
            foreach (int occupiedPos in positions)
            {
                if ((Inventory[occupiedPos].Id == item.Id &&
                    item.Type != ItemType.Equipment) ||
                    pos < occupiedPos)
                    return pos;
                pos++;
            }

            return pos;
        }

        public void UseItem(Item item)
        {
            if (selectedItem == null)
                return;
            
            // Check for equipment
            if (item.Type == ItemType.Equipment)
            {
                EquipmentManager.Instance.ToggleEquipment(item);
                return;
            }

            // Apply modifiers
            foreach (AttributeModifier mod in item.Modifiers)
                CharacterManager.Instance.AddModifier(mod);

            // Remove item if usable
            if (item.Type == ItemType.Usable)
            {
                int i = (int)selectedItemIndex.Y * tilesX + (int)selectedItemIndex.X;
                Quantities[item.Id]--;
                if (Quantities[item.Id] <= 0)
                {
                    Inventory.Remove(i);
                    Quantities.Remove(item.Id);
                    selectedItem = null;
                }
            }
        }

        public override void Move(Vector2 movement)
        {
            Vector2 finalPos = new Vector2(selectedItemIndex.X + movement.X, selectedItemIndex.Y + movement.Y);
            if (finalPos.X >= 0 &&
                finalPos.Y >= 0 &&
                finalPos.X < tilesX &&
                finalPos.Y < tilesY)
                selectedItemIndex = finalPos;

            CheckSelectedItem();
        }

        private Item GetItem(int x, int y)
        {
            int i = y * tilesX + x;
            if (Inventory.ContainsKey(i))
                return Inventory[i];
            return null;
        }

        private void CheckSelectedItem()
        {
            selectedItem = GetItem((int)selectedItemIndex.X, (int)selectedItemIndex.Y);
        }

        public override void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            if (!IsOpened)
                return;

            // Window
            int windowX = bounds.Width / 2 - Width / 2;
            spriteBatch.Draw(dialogTexture,
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

                    int slotX = lastX;
                    int slotY = itemY + y * (itemSize + margin);

                    spriteBatch.Draw(slotTexture,
                                 destinationRectangle: new Rectangle(slotX, slotY, itemSize, itemSize),
                                 color: color);

                    
                    Item item = GetItem(x, y);
                    if (item != null)
                    {
                        // Item sprite
                        Texture2D texture = Global.GetTexture(item.TextureName);
                        if (texture != null)
                        {
                            Rectangle rect = Utils.GetRectangleByGid(item.Gid, item.TextureTileSize, texture.Width);
                            spriteBatch.Draw(texture,
                                             new Rectangle(slotX + item.TextureTileSize / 4, slotY + item.TextureTileSize / 4, item.TextureTileSize, item.TextureTileSize),
                                             rect,
                                             Color.White);

                            if (EquipmentManager.Instance.IsItemEquipped(item.Id))
                            {
                                spriteBatch.Draw(slotTexture,
                                 destinationRectangle: new Rectangle(slotX + itemSize - 15, slotY + itemSize - 15, 10, 10),
                                 color: Color.Blue);
                            }
                        }

                        // Item quantity, if more than 1
                        if (item.Type != ItemType.Equipment && Quantities.ContainsKey(item.Id) && Quantities[item.Id] > 1)
                        {
                            string str = Quantities[item.Id].ToString();
                            Vector2 measure = Global.DialogFont.MeasureString(str);
                            spriteBatch.DrawString(Global.DetailFont, str, new Vector2(slotX + itemSize - measure.X - 2 + 1, slotY + itemSize - measure.Y + 1), new Color(Color.Black, 0.5f));
                            spriteBatch.DrawString(Global.DetailFont, str, new Vector2(slotX + itemSize - measure.X - 2, slotY + itemSize - measure.Y), Color.White);
                        }
                    }
                }
            }

            // Description frame
            itemX = lastX + itemSize + margin;
            spriteBatch.Draw(slotTexture,
                             destinationRectangle: new Rectangle(itemX, itemY, windowX + Width - itemX - margin, Height - margin * 2),
                             color: Color.White);

            // Item name
            if (selectedItem != null)
            {
                string str = Utils.GetCroppedString(selectedItem.Name, Global.DialogFont, windowX + Width - itemX - (margin * 3), Height - margin * 4)[0];
                spriteBatch.DrawString(Global.DialogFont, str, new Vector2(itemX + margin, itemY + margin), Color.Yellow);

                // Item description
                Vector2 measure = Global.DialogFont.MeasureString(str);
                str = Utils.GetCroppedString(selectedItem.Description, Global.DetailFont, windowX + Width - itemX - (margin * 3), Height - margin * 4 - measure.Y)[0];
                spriteBatch.DrawString(Global.DetailFont, str, new Vector2(itemX + margin, itemY + margin + measure.Y + 10), Color.White);
            }
        }

        public override void PerformAction()
        {
            if (selectedItem != null)
            {
                UseItem(selectedItem);
            }
        }

        // Singleton stuff
        private static InventoryManager instance;
        protected InventoryManager() : base()
        {
            slotTexture = Global.GetTexture("item_bg");

            Inventory = new Dictionary<int, Item>();
            Quantities = new Dictionary<int, int>();

            Width = 550;
            Height = 300;
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
