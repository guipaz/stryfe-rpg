using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Managers
{
    public class PauseManager
    {
        public bool Paused { get; set; }
        public bool Waiting { get; set; }

        private double currentDelay;

        public void Wait(int seconds)
        {
            currentDelay = seconds;
            Waiting = true;
            Paused = false;
        }

        public void Update(double timePassed)
        {
            if (!Paused && !Waiting)
                return;

            currentDelay -= timePassed;

            // Finished the waiting/pause
            if (currentDelay <= 0)
            {
                currentDelay = 0;
                Paused = false;
                Waiting = false;
                ScriptInterpreter.Instance.FinishedCommand();
            }
        }

        // Singleton stuff
        private static PauseManager instance;
        protected PauseManager()
        {
            Paused = false;
        }
        public static PauseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PauseManager();
                }
                return instance;
            }
        }
    }
}
