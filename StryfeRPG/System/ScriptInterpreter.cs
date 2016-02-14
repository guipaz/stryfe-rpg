using Microsoft.Xna.Framework;
using StryfeRPG.Managers;
using StryfeRPG.Models.Maps;
using StryfeRPG.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System
{
    public class ScriptInterpreter
    {
        private Script currentScript;
        private int currentCommand;
        private MapObject caller;

        public void RunScript(int id, MapObject caller)
        {
            currentScript = Global.GetScript(id);
            currentCommand = 0;
            this.caller = caller;

            NextCommand();
        }

        public bool IsScriptRunning()
        {
            return currentScript != null;
        }

        public void FinishedCommand()
        {
            if (currentScript != null)
                NextCommand();
        }

        private void NextCommand()
        {
            // If the commands are finished, finishes the script and dismisses the caller
            if (currentCommand >= currentScript.Commands.Count())
            {
                currentScript = null;
                caller.Dismiss();
                return;
            }

            ScriptCommand command = currentScript.Commands[currentCommand];
            currentCommand++;

            // Replace the macros
            List<string> arguments = new List<string>();
            foreach (string argument in command.Arguments)
            {
                arguments.Add(ReplaceMacros(argument));
            }

            // Run the command
            switch (command.Command)
            {
                case "message":
                    DialogManager.Instance.ActivateDialog(new Dialog(arguments[0], arguments.GetRange(1, arguments.Count() - 1)), caller);
                    break;
                case "wait":
                    PauseManager.Instance.Wait(int.Parse(arguments[0]));
                    break;
                case "teleport":
                    MapManager.Instance.Teleport(arguments[0], new Vector2(int.Parse(arguments[1]), int.Parse(arguments[2])), Utils.GetDirection(arguments[3]));
                    break;
                case "add_item":
                    InventoryManager.Instance.AddItem(int.Parse(arguments[0]), int.Parse(arguments[1]));
                    break;
            }
        }

        private string ReplaceMacros(string str)
        {
            char[] stoppers = new char[2] { ' ', '\'' };
            while (str.Contains('$'))
            {
                bool found = true;

                int macroIndex = str.IndexOf('$');
                int stopperIndex = str.IndexOfAny(stoppers);

                string macro = str.Substring(str.IndexOf('$') + 1, (stopperIndex != -1 ? stopperIndex : str.Count()) - str.IndexOf('$') - 1);
                
                switch (macro)
                {
                    case "player":
                        str = str.Replace("$player", Global.Player.Name);
                        break;
                    default:
                        found = false;
                        break;
                }

                if (!found)
                    break;
            }

            return str;
        }

        // Singleton stuff
        private static ScriptInterpreter instance;
        protected ScriptInterpreter() { }
        public static ScriptInterpreter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptInterpreter();
                }
                return instance;
            }
        }
    }
}
