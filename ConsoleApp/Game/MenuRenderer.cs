using System;
using System.Collections.Generic;

namespace ConsoleApp {
    
    public class MenuRenderer {

        private const int MenuLength = 40;

        private string _previousFrame = "";

        private readonly string _defaultStyle = Color.BlackBackground + Color.WhiteText;
        private readonly string _descriptionStyle = Color.BlackBackground + Color.YellowText;
        private readonly string _menuBorderTopBottom = Color.WhiteBackground + new string(' ', MenuLength) + Color.Reset;
        private readonly string _menuBorderLeftRight = Color.WhiteBackground + " " + Color.Reset;
        
        public void RenderMenu(MenuEventListener eventListener, MenuLevel level, int selectedRow) {
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
            RenderScreen(outputString);
            eventListener.EventListener(level, selectedRow, submenuList.Count);
        }

        private void RenderScreen(string currentFrame) {
            string[] prevFrame = _previousFrame.Split("\n");
            string[] currFrame = currentFrame.Split("\n");
            _previousFrame = currentFrame;
            if (prevFrame.Length != currFrame.Length) {
                Console.Clear();
                Console.WriteLine(currentFrame);
                return;
            }

            for (int i = 0; i < currFrame.Length; i++) {
                if (prevFrame[i] == currFrame[i]) {
                    continue;
                }

                Console.SetCursorPosition(0, i);
                Console.WriteLine(currFrame[i] + new string(' ', 50));
            }
        }
    }
}