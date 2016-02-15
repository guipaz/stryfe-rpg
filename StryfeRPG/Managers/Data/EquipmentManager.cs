using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StryfeCore.Models.Items;
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
        public List<Equipment> Equipped = new List<Equipment>();

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

            foreach (Equipment e in Equipped)
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
                    Equipped.Remove(e);

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

            Equipped.Add(equip);

            Utils.PrintStats();
        }

        public bool IsItemEquipped(int id)
        {
            foreach (Equipment e in Equipped)
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
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    slotX = x * (itemSize + marginIn * 2 + Global.TileSize);
                    slotY = y * (itemSize + marginIn);
                    spriteBatch.Draw(slotTexture,
                             destinationRectangle: new Rectangle(windowX + marginOut + slotX, windowY + marginOut + slotY, itemSize, itemSize),
                             color: Color.White);
                }
            }

            // Player image
            int imgX = windowX + marginOut + itemSize + marginIn;
            spriteBatch.Draw(Global.Player.Texture,
                            destinationRectangle: new Rectangle(imgX, windowY + Height / 2 - Global.TileSize / 2, Global.TileSize, Global.TileSize),
                            sourceRectangle: Utils.GetRectangleByGid(Global.Player.GetSprite(), Global.TileSize, Global.Player.Texture.Width),
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
