using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeCore.Models.Items;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
using StryfeRPG.System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers.Data
{
    public class EquipmentManager : WindowManager
    {
        // Data
        public Dictionary<int, Equipment> EquippedItems = new Dictionary<int, Equipment>();
        private int Accessory1 = -1;
        private int Accessory2 = -1;

        // Screen
        private Texture2D slotTexture;
        private int itemSize = 50;
        private int marginOut = 15;
        private int marginIn = 10;
        private int tilesX = 2;
        private int tilesY = 4;

        // Navigation
        private Vector2 selectedTile;
        private int selectedItem;

        public override void OpenWindow()
        {
            base.OpenWindow();

            CheckSelectedItem();
        }

        public override void Move(Vector2 movement)
        {
            Vector2 finalPos = new Vector2(selectedTile.X + movement.X, selectedTile.Y + movement.Y);
            if (finalPos.X >= 0 &&
                finalPos.Y >= 0 &&
                finalPos.X < tilesX &&
                finalPos.Y < tilesY)
                selectedTile = finalPos;

            CheckSelectedItem();
        }

        private void CheckSelectedItem()
        {
            selectedItem = GetEquipmentId((int)selectedTile.X, (int)selectedTile.Y);
        }

        private int GetEquipmentId(int x, int y)
        {
            EquipmentType type = GetEquipType(x, y);
            foreach (KeyValuePair<int, Equipment> e in EquippedItems)
            {
                if (type == e.Value.EquipType)
                {
                    if (type == EquipmentType.Accessory)
                        return x == 0 ? Accessory1 : Accessory2;
                    return e.Key;
                }
            }

            return -1;
        }

        public void ToggleEquipment(int inventoryId)
        {
            if (IsItemEquipped(inventoryId))
            {
                UnequipItem(inventoryId);
            } else
            {
                EquipItem(inventoryId);
            }
        }

        public void EquipItem(int inventoryId)
        {
            if (InventoryManager.Instance.Items.ContainsKey(inventoryId))
            {
                Equipment item = (Equipment)InventoryManager.Instance.Items[inventoryId];

                // Unequip the slot type
                UnequipSlot(item.EquipType);

                // Adds the modifiers
                foreach (AttributeModifier mod in item.Modifiers)
                    CharacterManager.Instance.AddModifier(mod);
                
                // Adds the item to the control dictionary
                EquippedItems.Add(inventoryId, item);

                // If it's an accessory, sets the reference
                if (item.EquipType == EquipmentType.Accessory)
                {
                    if (Accessory1 != -1)
                        Accessory2 = inventoryId;
                    else
                        Accessory1 = inventoryId;
                }

                // Checks the selected item
                CheckSelectedItem();
            }
        }

        public void UnequipItem(int inventoryId)
        {
            if (EquippedItems.ContainsKey(inventoryId))
            {
                Equipment item = EquippedItems[inventoryId];
                
                // Removes from the equipped control dictionary
                EquippedItems.Remove(inventoryId);

                // If it's an accessory, unsets the reference
                if (item.EquipType == EquipmentType.Accessory)
                {
                    if (Accessory1 == inventoryId)
                    {
                        Accessory1 = Accessory2;
                        Accessory2 = -1;
                    }
                    else
                    {
                        Accessory2 = -1;
                    }                        
                }

                // Adds the item back to the inventory
                InventoryManager.Instance.AddItem(item, 1, inventoryId, false);

                // Removes the modifiers
                CharacterManager.Instance.RemoveModifiers(item.Id);

                // Checks the selected item
                CheckSelectedItem();
            }
        }

        public void UnequipSlot(EquipmentType type)
        {
            foreach (KeyValuePair<int, Equipment> equipped in EquippedItems)
            {
                if (equipped.Value.EquipType == type)
                {
                    // Unequip if it's not an accessory or if there's two equipped
                    if (type != EquipmentType.Accessory ||
                        (Accessory1 != -1 && Accessory2 != -1))
                    {
                        UnequipItem(equipped.Key);
                        break;
                    }
                }
            }

            CheckSelectedItem();
        }
        
        public bool IsItemEquipped(int inventoryId)
        {
            foreach (KeyValuePair<int, Equipment> e in EquippedItems)
                if (e.Key == inventoryId)
                    return true;
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, double timePassed)
        {
            if (!IsOpened)
                return;

            WindowUtils.spriteBatch = spriteBatch;

            // Window
            Window window = new Window(bounds.Width / 2 - Width / 2, bounds.Height / 2 - Height / 2, Width, Height);
            window.Margin = 15;

            // Item slot
            int slotX = 0;
            int slotY = 0;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    // Gets the right color
                    Color color = Color.White;
                    if (selectedTile.X == x && selectedTile.Y == y)
                        color = Color.Cyan; // when the slot is selected

                    slotX = x * (itemSize + marginIn * 2 + Global.TileSize);
                    slotY = y * (itemSize + marginIn);

                    // Adds the slot window to the master window
                    Window slot = new Window(slotX, slotY, itemSize, itemSize, slotTexture, color);
                    window.AddChild(slot);

                    // Checks if there's any item
                    EquipmentType type = GetEquipType(x, y);
                    Item item = null;
                    foreach (KeyValuePair<int, Equipment> e in EquippedItems)
                    {
                        if (e.Value.EquipType == type)
                        {
                            if (type == EquipmentType.Accessory)
                            {
                                if (!(x == 0 && e.Key == Accessory1) && !(x == 1 && e.Key == Accessory2))
                                    continue;
                            }

                            item = e.Value;
                            break;
                        }
                    }

                    // Draws item sprite or thumbnail if nothing is equipped
                    if (item != null)
                    {
                        Texture2D texture = Global.GetTexture(item.TextureName);
                        if (texture != null)
                        {
                            Rectangle rect = Utils.GetRectangleByGid(item.Gid, item.TextureTileSize, texture.Width);
                            Window icon = new Window(item.TextureTileSize / 4,
                                                     item.TextureTileSize / 4,
                                                     item.TextureTileSize,
                                                     item.TextureTileSize,
                                                     texture,
                                                     Color.White,
                                                     rect);
                            slot.AddChild(icon);
                        }
                    } else
                    {
                        //TODO: thumbnail
                    }
                }               
                
            }
            
            // Player image
            Window playerImage = new Window(itemSize + marginIn, Height / 2 - Global.TileSize / 2, Global.TileSize, Global.TileSize,
                                       Global.Player.Texture,
                                       Color.White,
                                       Utils.GetRectangleByGid(Global.Player.GetSprite(FacingDirection.Down), Global.TileSize, Global.Player.Texture.Width));
            window.AddChild(playerImage);

            // Description frame
            Window description = new Window(playerImage.Bounds.X + Global.TileSize + itemSize + marginIn * 2,
                                            0,
                                            Width - playerImage.Bounds.X - Global.TileSize - itemSize - marginIn * 2 - marginOut * 2,
                                            Height - marginOut * 2,
                                            slotTexture,
                                            Color.White);
            window.AddChild(description);

            // Item name
            if (selectedItem != -1)
            {
                Item i = EquippedItems[selectedItem];

                int margin = 10;

                TextWindow nameWindow = new TextWindow(Global.DialogFont, i.Name,
                                                 new Vector2(margin, margin),
                                                 new Vector2(description.Bounds.Width - margin * 2, description.Bounds.Height - margin * 2),
                                                 Color.DarkSlateBlue);
                description.AddChild(nameWindow);

                // Item description
                Vector2 measure = nameWindow.Font.MeasureString(nameWindow.Text);
                TextWindow descriptionText = new TextWindow(Global.DetailFont, i.Description,
                                                        new Vector2(margin, measure.Y),
                                                        new Vector2(description.Bounds.Width - margin * 3, description.Bounds.Height - margin * 3 - measure.Y));
                description.AddChild(descriptionText);
            }

            // Draws the window
            WindowUtils.DrawWindow(window);
        }

        private EquipmentType GetEquipType(int x, int y)
        {
            EquipmentType type = EquipmentType.Accessory;
            if (x == 0)
            {
                switch (y)
                {
                    case 0:
                        type = EquipmentType.Helmet;
                        break;
                    case 1:
                        type = EquipmentType.Weapon;
                        break;
                    case 2:
                        type = EquipmentType.Cape;
                        break;
                    case 3:
                        type = EquipmentType.Accessory;
                        break;
                }
            }
            else
            {
                switch (y)
                {
                    case 0:
                        type = EquipmentType.Armor;
                        break;
                    case 1:
                        type = EquipmentType.Shield;
                        break;
                    case 2:
                        type = EquipmentType.Shoes;
                        break;
                    case 3:
                        type = EquipmentType.Accessory;
                        break;
                }
            }

            return type;
        }

        public override void PerformAction()
        {
            if (selectedItem != -1)
                ToggleEquipment(selectedItem);
        }

        // Singleton stuff
        private static EquipmentManager instance;
        protected EquipmentManager() : base()
        {
            slotTexture = Global.GetTexture("item_bg");

            Width = marginOut * 2 + itemSize * 2 + marginIn * 3 + Global.TileSize + 200;
            Height = marginOut * 2 + itemSize * 4 + marginIn * 3;
        }
        public static EquipmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EquipmentManager();
                }
                return instance;
            }
        }
    }
}
