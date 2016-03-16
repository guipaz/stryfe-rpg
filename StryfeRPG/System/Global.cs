using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StryfeRPG.Managers;
using StryfeRPG.Models.Characters;
using StryfeRPG.Models.Items;
using StryfeRPG.Models.Maps;
using StryfeRPG.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.System
{
    public static class Global
    {
        // Settings
        public static int TileSize = 64;

        public static TmxObject defaultObj;
        public static Tileset defaultTileset;

        // Utils references
        public static Viewport Viewport;
        public static Player Player;
        public static SpriteFont MapFont;
        public static SpriteFont DialogFont;
        public static SpriteFont DetailFont;
        
        // Loaded resources
        private static ContentManager Content;
        private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Song> Songs = new Dictionary<string, Song>();

        // Loaded data
        private static Dictionary<int, Dialog> Dialogs = new Dictionary<int, Dialog>();
        private static Dictionary<int, Script> Scripts = new Dictionary<int, Script>();
        private static Dictionary<int, Item> Items = new Dictionary<int, Item>();

        // Runtime data
        private static Dictionary<string, bool> Switches = new Dictionary<string, bool>();

        // Networking
        public static Dictionary<int, Player> NetPlayers = new Dictionary<int, Player>();
        
        // Getters
        public static Texture2D GetTexture(string name)
        {
            if (!Textures.ContainsKey(name))
                Textures.Add(name, Content.Load<Texture2D>(String.Format("Textures/{0}", name)));
            return Textures[name];
        }

        public static Song GetSong(string name)
        {
            if (!Songs.ContainsKey(name))
                Songs.Add(name, Content.Load<Song>(String.Format("Music/{0}", name)));
            return Songs[name];
        }

        public static Dialog GetDialog(int id)
        {
            if (Dialogs.ContainsKey(id))
                return Dialogs[id];
            return null;
        }

        public static Script GetScript(int id)
        {
            if (Scripts.ContainsKey(id))
                return Scripts[id];
            return null;
        }

        public static bool GetSwitch(string sw)
        {
            return Switches.ContainsKey(sw) ? Switches[sw] : false;
        }

        public static Item GetItem(int id)
        {
            if (Items.ContainsKey(id))
                return Items[id];
            return null;
        }

        public static void RetrieveAllItems()
        {
            foreach (KeyValuePair<int, Item> i in Items)
                InventoryManager.Instance.AddItem(i.Value, 1);
        }

        // Setters
        public static void SetSwitch(string sw, bool value)
        {
            Switches[sw] = value;
        }

        public static void SetDialogs(Dictionary<int, Dialog> dialogs)
        {
            Global.Dialogs = dialogs;
        }

        public static void SetScripts(Dictionary<int, Script> scripts)
        {
            Global.Scripts = scripts;
        }
        
        public static void SetItems(Dictionary<int, Item> items)
        {
            Global.Items = items;
        }

        public static void SetContent(ContentManager content)
        {
            Content = content;
        }
    }
}
