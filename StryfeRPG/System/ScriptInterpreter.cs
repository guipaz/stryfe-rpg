using Microsoft.Xna.Framework;
using StryfeCore.Models.Utils;
using StryfeRPG.Managers;
using StryfeRPG.Managers.GUI;
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
        private ScriptPage currentPage;

        private int currentCommand;
        private MapObject caller;
        
        public void RunScript(int id, MapObject caller)
        {
            currentCommand = 0;
            this.caller = caller;

            currentScript = Global.GetScript(id);
            currentPage = GetCurrentPage();
            
            NextCommand();
        }

        private ScriptPage GetCurrentPage()
        {
            ScriptPage finalPage = null;

            if (currentScript != null && currentScript.Pages.Count() > 0)
            {
                // Evaluate each condition in order
                foreach (ScriptPage page in currentScript.Pages)
                {
                    // Returns if there's no condition to be met
                    if ((page.Switch != null && Global.GetSwitch(page.Switch)) ||
                        
                        (page.Switch == null &&
                        page.Condition == null))
                    {
                        finalPage = page;
                        continue;
                    }

                    List<string> args = GetArgumentsList(page.Arguments);
                    // Checks conditions
                    switch (page.Condition)
                    {
                        case Constants.ConditionInteraction: // Verifies if there has been enough interactions with the caller
                            ObjectInfo info = caller.SavedInformation;
                            if (info != null)
                            {
                                int value = int.Parse(args[1]);
                                if (Compare(args[0], info.NumberOfInteractions, value))
                                    finalPage = page;
                            }
                            break;
                        case Constants.ConditionHasItem:
                            if (InventoryManager.Instance.HasItem(int.Parse(args[0]), int.Parse(args[1])))
                                finalPage = page;
                            break;
                    }
                }
            }

            return finalPage;
        }

        private bool Compare(string comparison, int value1, int value2)
        {
            return (comparison == "!=" && value1 != value2) ||
                (comparison == "==" && value1 == value2) ||
                (comparison == ">" && value1 > value2) ||
                (comparison == "<" && value1 < value2) ||
                (comparison == ">=" && value1 >= value2) ||
                (comparison == "<=" && value1 <= value2);
        }

        private List<string> GetArgumentsList(string args)
        {
            return args != null ? new List<string>(args.Split(',')) : null;
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
            if (currentPage == null || currentCommand >= currentPage.Commands.Count())
            {
                currentScript = null;
                caller.Dismiss();
                return;
            }

            ScriptCommand command = currentPage.Commands[currentCommand];
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
                case Constants.CommandMessage:
                    DialogManager.Instance.ActivateDialog(new Dialog(arguments[0], arguments.GetRange(1, arguments.Count() - 1)), caller);
                    break;
                case Constants.CommandWait:
                    PauseManager.Instance.Wait(int.Parse(arguments[0]));
                    break;
                case Constants.CommandTeleport:
                    MapManager.Instance.Teleport(arguments[0], new Vector2(int.Parse(arguments[1]), int.Parse(arguments[2])), Utils.GetDirection(arguments[3]));
                    break;
                case Constants.CommandAddItem:
                    InventoryManager.Instance.AddItem(int.Parse(arguments[0]), int.Parse(arguments[1]));
                    break;
                case Constants.CommandRemoveItem:
                    InventoryManager.Instance.RemoveItem(int.Parse(arguments[0]), int.Parse(arguments[1]));
                    break;
                case Constants.CommandAlert:
                    QuickMessageManager.Instance.ShowMessage(new QuickMessage(arguments[0], arguments[1]));
                    FinishedCommand();
                    break;
                case Constants.CommandSetSwitch:
                    Global.SetSwitch(arguments[0], bool.Parse(arguments[1]));
                    FinishedCommand();
                    break;
            }
        }

        private string ReplaceMacros(string str)
        {
            char[] stoppers = new char[2] { ' ', '\'' };
            while (str.Contains('$'))
            {
                bool found = true;
                int stopperIndex = str.IndexOfAny(stoppers);

                string macro = str.Substring(str.IndexOf('$'), (stopperIndex != -1 ? stopperIndex : str.Count()) - str.IndexOf('$'));
                
                switch (macro)
                {
                    case Constants.MacroPlayerName:
                        str = str.Replace(Constants.MacroPlayerName, Global.Player.Name);
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
