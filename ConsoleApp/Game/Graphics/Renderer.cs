using System;
using System.Collections.Generic;
using ConsoleApp.Utils;

namespace ConsoleApp.Graphics {
    
    public abstract class Renderer {

        public int FieldWidth;
        public string BottomAIndexes, Separator;
        public string EmptySpace = new string(' ', 9);
        public string PreviousFrame = "";

        protected string GetBottomIndexes(int size) {
            string str = "";
            for (int i = 0; i < size; i++) {
                str += $" {DataUtils.Alphabet[i]} ";
            }

            return str;
        }

        protected string GetLevelDescription(string mode, string currPlayer, string player1Name = "", string player2Name = "") {
            if (mode != "Board Building") {
                return DataUtils.System + $"   Game mode: {mode}, {player1Name} against {player2Name}" +
                       Color.Reset + "\n";
            }

            return DataUtils.System + $"   {currPlayer}, please make up the battle formation for your ships" + Color.Reset + "\n";
        }

        protected string GetFieldCoordinatesTemplate(string mode) {
            string template = "";
            if (mode != "Board Building") {
                template += DataUtils.Default + $"   {Separator}{EmptySpace}{Separator} \n";
                template += $"   {BottomAIndexes}{EmptySpace}{BottomAIndexes} \n";
            }
            else {
                template += DataUtils.Default + $"   {Separator}\n";
                template += $"   {BottomAIndexes} \n";
            }

            return template;
        }

        protected string GetFieldFooter(string mode) {
            string fieldFooterString = "";
            if (mode != "Board Building" && mode != "AI vs AI") {
                fieldFooterString += DataUtils.Default + $"   Enemy field{new string(' ', Separator.Length - 2)}Your field \n";
                fieldFooterString += $"   {Separator}{EmptySpace}{Separator} \n";
            }
            else if (mode == "AI vs AI") {
                fieldFooterString += DataUtils.Default + $"   Beavis field{new string(' ', Separator.Length - 3)}Butthead field \n";
                fieldFooterString += $"   {Separator}{EmptySpace}{Separator} \n";
            }
            else {
                fieldFooterString += DataUtils.Default + $"   {Separator}" + Color.Reset + "\n";
            }
            
            return fieldFooterString;
        }

        protected string GetBattleMenu(IReadOnlyList<string> menuOptions, int row, int rowCount) {
            string battleMenu = "";
            for (int z = 0; z < menuOptions.Count; z++) {
                battleMenu += Color.BlackBackground + "  " + Color.Reset;
                if (row - rowCount == z) {
                    battleMenu += Color.BlueBackground + Color.WhiteText +
                                  $" {menuOptions[z]}{new string(' ', (FieldWidth * 3) - menuOptions[z].Length)}" +
                                  Color.Reset + "\n";
                    continue;
                }

                battleMenu += DataUtils.System + $" {menuOptions[z]}" + Color.Reset + "\n";
            }

            return battleMenu;
        }

        protected void RenderScreen(string currentFrame) {
            string[] prevFrame = PreviousFrame.Split("\n");
            string[] currFrame = currentFrame.Split("\n");
            PreviousFrame = currentFrame;
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
                Console.WriteLine(currFrame[i] + new string(' ', 75));
            }
        }
    }
}