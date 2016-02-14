using Microsoft.Xna.Framework.Media;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class AudioManager
    {
        public string currentSong { get; set; }
        
        public void PlaySong(string song)
        {
            if (song == currentSong)
                return;

            currentSong = song;

            Song songObject = Global.GetSong(song);
            if (songObject != null)
                MediaPlayer.Play(songObject);
        }

        // Singleton stuff
        private static AudioManager instance;
        protected AudioManager() { }
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }
                return instance;
            }
        }
    }
}
