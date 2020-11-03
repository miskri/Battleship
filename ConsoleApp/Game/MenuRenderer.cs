using System;
using System.Collections.Generic;

namespace ConsoleApp {
    
    public class MenuRenderer {

        private const int MenuLength = 40;
        private readonly MenuManager _menuManager;

        private readonly string _defaultStyle = Color.BlackBackground + Color.WhiteText;
        private readonly string _descriptionStyle = Color.BlackBackground + Color.YellowText;
        private readonly string _menuBorderTopBottom = Color.WhiteBackground + new string(' ', MenuLength) + Color.Reset;
        private readonly string _menuBorderLeftRight = Color.WhiteBackground + " " + Color.Reset;
        
        public MenuRenderer(MenuManager menuManager) {
            _menuManager = menuManager;
        }
        
        public void RenderMenu(MenuEventListener eventListener, MenuLevel level, int selectedRow) {
            Console.Clear();
            string outputString = "";
            List<string> submenuList = level.SubmenuList;
            outputString += _menuBorderTopBottom + "\n";
            
            if (level.LevelDescription != "") {
                outputString += $"{_menuBorderLeftRight} " + _descriptionStyle + level.LevelDescription + _defaultStyle +
                                $"{new string(' ', MenuLength - 3 - level.LevelDescription.Length)}" 
                                + _menuBorderLeftRight + "\n";
            }

            for (int i = 0; i < submenuList.Count; i++) {
                string emptySpace = new string(' ', MenuLength - 3 - submenuList[i].Length);
                if (i == selectedRow) {
                    outputString += _menuBorderLeftRight + 
                                    $"{Color.BlueBackground + Color.WhiteText} {submenuList[i]}{emptySpace}" +
                                    _defaultStyle + _menuBorderLeftRight + "\n";
                    continue;
                }

                outputString += _menuBorderLeftRight + $" {submenuList[i]}{emptySpace}" + _menuBorderLeftRight + "\n";
            }

            outputString += _menuBorderTopBottom + "\n";
            Console.WriteLine(outputString);
            eventListener.EventListener(level, selectedRow, submenuList.Count);
        }
    }
}