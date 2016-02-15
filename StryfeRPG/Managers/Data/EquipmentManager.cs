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
        public List<Equipment> EquippedItems = new List<Equipment>();

        // Screen
        private Texture2D slotTexture;
        private int itemSize = 50;
        private int marginOut = 15;
        private int marginIn = 10;

        public void Move(Vector2 movement)
        {

        }

        public void ToggleEquipment(Item item)
        {
            Equipment equip = (Equipment)item;

            Item removeItem = null;
            bool sameItem = false;
            int accessories = 0; // can equip 2 accessories

            foreach (Equipment e in EquippedItems)
            {
                if (e.EquipType == equip.EquipType)
                {
                    if (e.Id == equip.Id)
                        sameItem = true;

                    if (e.EquipType == EquipmentType.Accessory)
                    {
                        accessories++;
                        if (accessories < 2 && !sameItem)
                            continue;
                    }

                    removeItem = e;
                    EquippedItems.Remove(e);

                    break;
                }
            }
            
            if (removeItem != null)
            {
                CharacterManager.Instance.RemoveModifiers(removeItem.Id);
            }

            if (sameItem)
            {
                Utils.PrintStats();
                return;
            }
                

            foreach (AttributeModifier mod in equip.Modifiers)
                CharacterManager.Instance.AddModifier(mod);

            EquippedItems.Add(equip);

            Utils.PrintStats();
        }

        public bool IsItemEquipped(int id)
        {
            foreach (Equipment e in EquippedItems)
                if (e.Id == id)
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
            int accessoryAux = 0; // control for more than one accessory
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    // Slot
                    slotX = x * (itemSize + marginIn * 2 + Global.TileSize);
                    slotY = y * (itemSize + marginIn);
                    spriteBatch.Draw(slotTexture,
                             destinationRectangle: new Rectangle(windowX + marginOut + slotX, windowY + marginOut + slotY, itemSize, itemSize),
                             color: Color.White);

                    // Gets which equip type is
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

                    Item item = null;
                    foreach (Equipment e in EquippedItems)
                    {
                        if (e.EquipType == type)
                        {
                            if (type == EquipmentType.Accessory)
                            {
                                if (accessoryAux == 1)
                                {
                                    accessoryAux++; // it's ugly but it works
                                    continue;
                                }

                                accessoryAux++;
                            }

                            item = e;
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

        public override void PerformAction()
        {
            throw new NotImplementedException();
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
