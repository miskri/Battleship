using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp {
    
    public class MenuRenderer : Renderer {

        private int _menuLength = 50;
        private readonly string _menuBorderLeftRight = Color.WhiteBackground + " " + Color.Reset;
        
        public void RenderMenu(MenuProperties props) {
            string outputString = "";
            string menuBorderTopBottom = GetMenuHeaderFooter(props.Level.SubmenuList, props.Level.LevelDescription);
            List<string> submenuList = props.Level.SubmenuList;
            outputString += menuBorderTopBottom + "\n";
            
            if (props.Level.LevelDescription != "") {
                outputString += $"{_menuBorderLeftRight} " + Data.Default + props.Level.LevelDescription + Data.Menu + 
                                $"{new string(' ', _menuLength - 3 - props.Level.LevelDescription.Length)}" 
                                + _menuBorderLeftRight + "\n";
            }

            for (int i = 0; i < submenuList.Count; i++) {
                string emptySpace = new string(' ', _menuLength - 3 - submenuList[i].Length);
                if (props.Level.SubmenuList[i] == "Back") {
                    outputString += _menuBorderLeftRight + new string('—', _menuLength - 2) + _menuBorderLeftRight + "\n";
                }
                if (i == props.SelectedRow) {

                    outputString += _menuBorderLeftRight + 
                                    $"{Color.BlueBackground + Color.WhiteText} {submenuList[i] + emptySpace}" +
                                    Data.Menu + _menuBorderLeftRight + "\n";
                    continue;
                }
                outputString += _menuBorderLeftRight + $" {submenuList[i] + emptySpace}" + _menuBorderLeftRight + "\n";
            }

            outputString += menuBorderTopBottom + "\n";
            RenderScreen(outputString);
        }

        public void RenderSettingsSetup(string[] options, string message, int selectedRow) {
            string outputString = "";
            string menuBorderTopBottom = GetMenuHeaderFooter(options, message);
            
            outputString += menuBorderTopBottom + "\n";
            
            outputString += $"{_menuBorderLeftRight} " + Data.Default + message + Data.Menu + 
                                $"{new string(' ', _menuLength - 3 - message.Length)}" + 
                                _menuBorderLeftRight + "\n";

            for (int i = 0; i < options.Length; i++) {
                string emptySpace = new string(' ', _menuLength - 3 - options[i].Length);
                if (options[i] == "Return to settings menu") {
                    outputString += _menuBorderLeftRight + new string('—', _menuLength - 2) + _menuBorderLeftRight + "\n";
                }
                if (i == selectedRow) {
                    outputString += _menuBorderLeftRight + 
                                    $"{Color.BlueBackground + Color.WhiteText} {options[i] + emptySpace}" +
                                    Data.Menu + _menuBorderLeftRight + "\n";
                    continue;
                }
                outputString += _menuBorderLeftRight + $" {options[i] + emptySpace}" + _menuBorderLeftRight + "\n";
            }

            outputString += menuBorderTopBottom + "\n";
            RenderScreen(outputString);
        }

        private string GetMenuHeaderFooter(IEnumerable<string> items, string headerMessage) {
            int size = items.Select(item => item.Length).Concat(new[] {0}).Max();
            if (headerMessage.Length > size) size = headerMessage.Length;
            _menuLength = size + 4;
            return Color.WhiteBackground + new string(' ', _menuLength) + Color.Reset;
        }
    }
}