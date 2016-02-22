using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using StryfeCore.Models.Items;
using StryfeCore.Models.Utils;
using StryfeRPG.Managers.Data;
using StryfeRPG.Managers.GUI;
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
        public Dictionary<int, Item> Items { get; set; } // Key: inventory id - Value: item
        private Dictionary<int, int> Quantities; // Key: inventory id - Value: quantity

        // Items positioning
        private List<int> CurrentInventory;
        private ItemType CurrentTabType;

        private List<int> UsableInventory;
        private List<int> EquipInventory;
        private List<int> MiscInventory;

        private Item selectedItem;
        private Vector2 selectedItemIndex;

        // Screen management
        private int tilesX = 4;
        private int tilesY = 4;
        
        private Texture2D slotTexture;

        // Public API
        public override void OpenWindow()
        {
            base.OpenWindow();
            CheckSelectedItem();
        }

        public void AddItem(int id, int quantity = 1)
        {
            Item item = Global.GetItem(id);
            if (item != null)
            {
                AddItem(item, quantity);
            }
        }

        public void AddItem(Item item, int quantity = 1, int inventoryId = -1, bool showAlert = true)
        {
            if (item != null)
            {
                inventoryId = inventoryId == -1 ? GetAvailableInventoryId(item) : inventoryId;

                Console.WriteLine("Added item: {0} - {1}", item.Name, inventoryId);

                Items[inventoryId] = item;
                Quantities[inventoryId] = Quantities.ContainsKey(inventoryId) ? Quantities[inventoryId] + quantity : quantity;

                List<int> positions = GetPositionList(item.Type);
                if (!positions.Contains(inventoryId))
                    GetPositionList(item.Type).Add(inventoryId);

                if (showAlert)
                    QuickMessageManager.Instance.ShowMessage(new QuickMessage("Item gained", string.Format("{0} x{1}", item.Name, quantity)));
            }
            
            ScriptInterpreter.Instance.FinishedCommand();
        }

        public void RemoveItem(int id, int quantity = 1)
        {
            Item item = Global.GetItem(id);
            if (item != null)
            {
                RemoveItem(item, quantity);
            }
        }

        public void RemoveItem(Item item, int quantity = 1, bool showAlert = true)
        {
            int inventoryId = -1;
            if (item != null)
            {
                foreach (KeyValuePair<int, Item> i in Items)
                {
                    if (item.Id == i.Value.Id)
                    {
                        inventoryId = i.Key;
                        break;
                    }
                }
            }

            if (inventoryId != -1)
            {
                EquipmentManager.Instance.UnequipItem(inventoryId);
                Quantities[inventoryId] -= quantity;
                if (Quantities[inventoryId] <= 0)
                {
                    Items.Remove(inventoryId);
                    Quantities.Remove(inventoryId);

                    GetPositionList(item.Type).Remove(inventoryId);
                }

                if (showAlert)
                    QuickMessageManager.Instance.ShowMessage(new QuickMessage("Item removed", string.Format("{0} x{1}", item.Name, quantity)));
            }
            
            ScriptInterpreter.Instance.FinishedCommand();
        }
        
        public void UseItem(int inventoryId)
        {
            if (inventoryId == -1)
                return;

            Item item = Items[inventoryId];
            
            // Check for equipment
            if (item.Type == ItemType.Equipment)
            {
                EquipmentManager.Instance.ToggleEquipment(inventoryId);
            }
            else
            {
                // Apply modifiers
                foreach (AttributeModifier mod in item.Modifiers)
                    CharacterManager.Instance.AddModifier(mod);
            }

            // Remove item from inventory
            Quantities[inventoryId]--;
            if (Quantities[inventoryId] <= 0)
            {
                Items.Remove(inventoryId);
                Quantities.Remove(inventoryId);

                GetPositionList(item.Type).Remove(inventoryId);
            }

            Console.WriteLine("Used item: {0} - {1}", item.Name, inventoryId);

            CheckSelectedItem();
        }
        
        public override void Move(Vector2 movement)
        {
            // Checks if it's entering the tabs mode
            if (movement.Y != 0 && selectedItemIndex.Y + movement.Y == -1)
            {
                int x = 0;
                switch (CurrentTabType)
                {
                    case ItemType.Usable:
                        x = 0;
                        break;
                    case ItemType.Equipment:
                        x = 1;
                        break;
                    default:
                        x = 2;
                        break;
                }

                selectedItemIndex = new Vector2(x, -1);
            } else if (selectedItemIndex.Y == -1 && movement.Y != 1) // Checks if it's in tabs mode and not exiting
            {
                int x = Math.Min((int)(selectedItemIndex.X + movement.X), 2);
                if (x < 0)
                    x = 0;
                selectedItemIndex = new Vector2(x, -1);
            } else if (selectedItemIndex.Y == -1 && movement.Y == 1) // Checks if it's exiting tabs mode
            {
                selectedItemIndex = Vector2.Zero;
            } else // Otherwise, checks the slots movement as usual
            {
                Vector2 finalPos = new Vector2(selectedItemIndex.X + movement.X, selectedItemIndex.Y + movement.Y);
                if (finalPos.X >= 0 &&
                    finalPos.Y >= -1 && // tabs
                    finalPos.X < tilesX &&
                    finalPos.Y < tilesY)
                    selectedItemIndex = finalPos;
            }

            CheckSelectedItem();
        }

        public bool HasItem(int itemId, int quantity = 1)
        {
            int currentQuantity = 0;

            // Checks inventory
            foreach (KeyValuePair<int, Item> i in Items)
            {
                if (i.Value.Id == itemId)
                {
                    currentQuantity += Quantities[i.Key];
                }
            }

            // Checks equipments
            foreach (KeyValuePair<int, Equipment> i in EquipmentManager.Instance.EquippedItems)
            {
                if (i.Value.Id == itemId)
                    currentQuantity++;
            }

            return currentQuantity >= quantity;
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

            // Tabs
            int tabMargin = 10;

            int tabWidth = tabMargin * 2 + (int)Global.DetailFont.MeasureString("Usable").X;
            int tabHeight = tabMargin * 2 + (int)Global.DetailFont.MeasureString("Usable").Y;

            int tabX = windowX;
            int tabY = bounds.Height / 2 - Height / 2 - tabHeight;

            spriteBatch.Draw(dialogTexture,
                             destinationRectangle: new Rectangle(tabX, tabY, tabWidth, tabHeight),
                             color: new Color(Color.White, CurrentTabType == ItemType.Usable ? 0.8f : 0.5f)); // Usable
            spriteBatch.DrawString(Global.DetailFont, "Usable", new Vector2(tabX + tabMargin, tabY + tabMargin), selectedItemIndex.Y == -1 && CurrentTabType == ItemType.Usable ? Color.White : Color.LightGray);

            tabX = tabX + tabMargin + tabWidth;
            tabWidth = tabMargin * 2 + (int)Global.DetailFont.MeasureString("Equips").X;

            spriteBatch.Draw(dialogTexture,
                             destinationRectangle: new Rectangle(tabX, tabY, tabWidth, tabHeight),
                             color: new Color(Color.White, CurrentTabType == ItemType.Equipment ? 0.8f : 0.5f)); // Equips
            spriteBatch.DrawString(Global.DetailFont, "Equips", new Vector2(tabX + tabMargin, tabY + tabMargin), selectedItemIndex.Y == -1 && CurrentTabType == ItemType.Equipment ? Color.White : Color.LightGray);

            tabX = tabX + tabMargin + tabWidth;
            tabWidth = tabMargin * 2 + (int)Global.DetailFont.MeasureString("Misc").X;

            spriteBatch.Draw(dialogTexture,
                             destinationRectangle: new Rectangle(tabX, tabY, tabWidth, tabHeight),
                             color: new Color(Color.White, CurrentTabType == ItemType.Misc ? 0.8f : 0.5f)); // Misc
            spriteBatch.DrawString(Global.DetailFont, "Misc", new Vector2(tabX + tabMargin, tabY + tabMargin), selectedItemIndex.Y == -1 && CurrentTabType == ItemType.Misc ? Color.White : Color.LightGray);

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
                    if (GetInventoryId(x, y) != -1)
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
                UseItem(GetInventoryId((int)selectedItemIndex.X, (int)selectedItemIndex.Y));
            }
        }
        
        // Private methods
        private int GetAvailableInventoryId(Item item)
        {
            List<int> ids = Items.Keys.ToList();
            ids.AddRange(EquipmentManager.Instance.EquippedItems.Keys.ToList());
            ids.Sort();

            int pos = 0;
            foreach (int occupiedId in ids)
            {
                if (Items.ContainsKey(occupiedId) && Items[occupiedId].Id == item.Id)
                {
                    if (item.Type != ItemType.Equipment)
                        return occupiedId;
                }

                pos++;
            }

            return ids.Contains(pos) ? pos + 1 : pos;
        }

        private List<int> GetPositionList(ItemType type)
        {
            switch (type)
            {
                case ItemType.Usable:
                    return UsableInventory;
                case ItemType.Equipment:
                    return EquipInventory;
                default:
                    return MiscInventory;
            }
        }

        private Item GetItem(int x, int y)
        {
            int inventoryId = GetInventoryId(x, y);

            if (Items.ContainsKey(inventoryId))
                return Items[inventoryId];
            return null;
        }

        private int GetInventoryId(int x, int y)
        {
            if (y == -1)
                return -1;

            int pos = y * tilesX + x;
            if (CurrentInventory.Count() <= pos)
                return -1;

            return CurrentInventory[pos];
        }

        private void CheckSelectedItem()
        {
            // Checks if the tabs are being moved
            if (selectedItemIndex.Y == -1)
            {
                // If they are, gets which tab should be showing now
                if (selectedItemIndex.X > 2)
                    selectedItemIndex = new Vector2(2, selectedItemIndex.Y);

                if (selectedItemIndex.X == 0)
                    CurrentTabType = ItemType.Usable;
                else
                    if (selectedItemIndex.X == 1)
                    CurrentTabType = ItemType.Equipment;
                else
                    CurrentTabType = ItemType.Misc;

                switch (CurrentTabType)
                {
                    case ItemType.Usable:
                        CurrentInventory = UsableInventory;
                        break;
                    case ItemType.Equipment:
                        CurrentInventory = EquipInventory;
                        break;
                    default:
                        CurrentInventory = MiscInventory;
                        break;
                }

                return;
            }

            // If it's not a tab, gets the selected item
            selectedItem = GetItem((int)selectedItemIndex.X, (int)selectedItemIndex.Y);
        }
        
        // Singleton stuff
        private static InventoryManager instance;
        protected InventoryManager() : base()
        {
            slotTexture = Global.GetTexture("item_bg");

            Items = new Dictionary<int, Item>();
            Quantities = new Dictionary<int, int>();

            UsableInventory = new List<int>();
            EquipInventory = new List<int>();
            MiscInventory = new List<int>();

            CurrentInventory = UsableInventory;
            CurrentTabType = ItemType.Usable;

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
