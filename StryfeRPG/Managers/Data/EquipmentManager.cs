using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeCore.Models.Items;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
using StryfeRPG.System;
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

            // Window
            int windowX = bounds.Width / 2 - Width / 2;
            int windowY = bounds.Height / 2 - Height / 2;
            spriteBatch.Draw(dialogTexture,
                             destinationRectangle: new Rectangle(windowX, windowY, Width, Height),
                             color: new Color(Color.White, 0.8f));

            // Item slot
            int slotX = 0;
            int slotY = 0;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    // Slot
                    Color color = Color.White;
                    int i = (int)y * tilesX + (int)x;
                    //if (Inventory.ContainsKey(i) && Inventory[i] != null)
                    //    color = Color.LightBlue; // when there's an item in the slot

                    if (selectedTile.X == x && selectedTile.Y == y)
                        color = Color.Cyan; // when the slot is selected

                    slotX = x * (itemSize + marginIn * 2 + Global.TileSize);
                    slotY = y * (itemSize + marginIn);
                    spriteBatch.Draw(slotTexture,
                             destinationRectangle: new Rectangle(windowX + marginOut + slotX, windowY + marginOut + slotY, itemSize, itemSize),
                             color: color);

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
                            spriteBatch.Draw(texture,
                                             new Rectangle(windowX + marginOut + slotX + item.TextureTileSize / 4,
                                                           windowY + marginOut + slotY + item.TextureTileSize / 4,
                                                           item.TextureTileSize, item.TextureTileSize),
                                             rect,
                                             Color.White);
                        }
                    } else
                    {
                        //TODO: thumbnail
                    }
                }               
                
            }

            // Player image
            int imgX = windowX + marginOut + itemSize + marginIn;
            spriteBatch.Draw(Global.Player.Texture,
                            destinationRectangle: new Rectangle(imgX, windowY + Height / 2 - Global.TileSize / 2, Global.TileSize, Global.TileSize),
                            sourceRectangle: Utils.GetRectangleByGid(Global.Player.GetSprite(FacingDirection.Down), Global.TileSize, Global.Player.Texture.Width),
                            color: Color.White);

            // Description frame
            int descriptionX = imgX + Global.TileSize + itemSize + marginIn * 2;
            spriteBatch.Draw(slotTexture,
                             destinationRectangle: new Rectangle(descriptionX, windowY + marginOut, Width + windowX - descriptionX - marginOut, Height - marginOut * 2),
                             color: Color.White);
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
