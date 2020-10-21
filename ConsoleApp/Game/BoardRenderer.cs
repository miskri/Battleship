using System;
using System.Collections.Generic;

namespace ConsoleApp {
    public class BoardRenderer {
        public readonly char[] Alphabet = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public List<string> BattleHistory = new List<string>();
        public BattleEventListener EventListener;

        private readonly string _defaultStyle = Color.BlackBackground + Color.YellowText;
        private readonly string _systemStyle = Color.BlackBackground + Color.GreenText;
        private readonly string _fieldCell = Color.BlueBackground + "   " + Color.Reset;
        private readonly string _shipCell = Color.GrayDarkBackground + "   " + Color.Reset;
        private readonly string _collisionCell = Color.RedBackground + Color.BlackText + " X " + Color.Reset;
        private readonly string _missCell = Color.BlueLightBackground + Color.BlackText + " ♦ " + Color.Reset;
        private string _bottomAIndexes, _separator;


        private readonly string _emptySpace = new string(' ', 9);
        private readonly string[] _bottomOptions = {"Save game", "Main Menu", "Quit"};
        private readonly string _gameMode, _playerOneName, _playerTwoName;

        private readonly int[] _fieldSize;


        public BoardRenderer(string gameMode, int[] fieldSize, string playerOneName = null,
            string playerTwoName = null) {
            _gameMode = gameMode;
            _fieldSize = fieldSize;
            _bottomAIndexes = GetBottomIndexes(fieldSize[0]);
            _separator = new string('-', fieldSize[0] * 3);

            _playerOneName = playerOneName;
            _playerTwoName = playerTwoName;

            BattleHistory.Add($"{playerTwoName} is ready!");
            BattleHistory.Add($"{playerOneName} is ready!");
        }

        private string GetBottomIndexes(int size) {
            string str = "";
            for (int i = 0; i < size; i++) {
                str += $" {Alphabet[i]} ";
            }

            return str;
        }

        public void RenderBoard(string[,] playerField, string[,] enemyField, int selectorX, int selectorY, string currentPlayer) {
            string outputString = "";
            Console.Clear();
            outputString += GetLevelDescription();
            outputString += GetFieldTemplate();

            for (int x = 0; x < _fieldSize[1]; x++) {
                outputString += GetField(playerField, enemyField, selectorX, selectorY, x);
            }

            outputString += _defaultStyle + $"   Enemy field{new string(' ', _separator.Length - 2)}Your field" +
                            Color.Reset + "\n";
            outputString += _defaultStyle + $"   {_separator}{_emptySpace}{_separator}" + Color.Reset + "\n";

            outputString += _defaultStyle + "   Selected quad: " + Color.Reset;
            if (selectorY >= 0 && selectorX >= 0 && selectorY < _fieldSize[0] && selectorX < _fieldSize[1]) {
                outputString += _defaultStyle + Alphabet[selectorY] + $"{selectorX + 1}" + Color.Reset;
            }

            outputString += "\n";
            outputString += _defaultStyle + $"   This is {currentPlayer} turn!" + Color.Reset + "\n";
            outputString += _defaultStyle + $"   {_separator}" + Color.Reset + "\n";
            outputString += _defaultStyle + "   Battle History:" + Color.Reset + "\n";
            outputString += _defaultStyle + $"   {BattleHistory[^2]}" + Color.Reset + "\n";
            outputString += _defaultStyle + $"   {BattleHistory[^1]}" + Color.Reset + "\n";
            outputString += _defaultStyle + $"   {_separator}" + Color.Reset + "\n";

            for (int z = 0; z < _bottomOptions.Length; z++) {
                outputString += Color.BlackBackground + "  " + Color.Reset;
                if (selectorX - _fieldSize[1] == z) {
                    outputString += Color.BlueBackground + Color.WhiteText +
                                    $" {_bottomOptions[z]}{new string(' ', (_fieldSize[0] * 3) - _bottomOptions[z].Length)}" +
                                    Color.Reset + "\n";
                    continue;
                }

                outputString += _systemStyle + $" {_bottomOptions[z]}" + Color.Reset + "\n";
            }

            Console.WriteLine(outputString);
            EventListener.BoardEventListener(playerField, enemyField, selectorX, selectorY, currentPlayer);
        }

        private string GetLevelDescription() {
            if (_gameMode != "Board Building") {
                return _systemStyle + $"   Game mode: {_gameMode}, {_playerOneName} against {_playerTwoName}" +
                       Color.Reset + "\n";
            }

            return _systemStyle + $"   Make up the battle formation for your ships" + Color.Reset + "\n";
        }

        private string GetFieldTemplate() {
            string template = "";
            if (_gameMode != "Board Building") {
                template += _defaultStyle + $"   {_separator}{_emptySpace}{_separator} \n";
                template += $"   {_bottomAIndexes}{_emptySpace}{_bottomAIndexes} \n";
            }
            else {
                template += _defaultStyle + $"{_separator}\n";
                template += $"   {_bottomAIndexes} \n";
            }

            return template;
        }

        private string GetField(string[,] playerField, string[,] enemyField, int selectorX, int selectorY, int x) {
            string fieldString = "";
            if (_gameMode != "Board Building" && _gameMode != "AI vs AI") {
                fieldString += DrawEnemyField(enemyField, selectorX, selectorY, x);
                fieldString += _defaultStyle + "      " + Color.Reset;
                fieldString += DrawPlayerField(playerField, x);
            }
            else if (_gameMode == "AI vs AI") {
                fieldString += DrawPlayerField(playerField, x);
                fieldString += _defaultStyle + "      " + Color.Reset;
                fieldString += DrawPlayerField(enemyField, x);
            }
            else {
                fieldString += DrawDefaultField(playerField, x);
            }
            fieldString += "\n";
            return fieldString;
        }

        private string DrawEnemyField(string[,] enemyField, int selectorX, int selectorY, int x) {
            string enemyFieldString = "";
            enemyFieldString += _defaultStyle + (x < 9 ? $" {x + 1} " : $"{x + 1} ") + Color.Reset;
            for (int y = 0; y < _fieldSize[0]; y++) {
                if (x == selectorX && y == selectorY) {
                    switch (enemyField[x, y]) {
                        case "collision":
                            enemyFieldString += Color.WhiteBackground + Color.BlackText + " X " + Color.Reset;
                            break;
                        case "miss":
                            enemyFieldString += Color.WhiteBackground + Color.BlackText + " ♦ " + Color.Reset;
                            break;
                        default:
                            enemyFieldString += Color.WhiteBackground + "   " + Color.Reset;
                            break;
                    }
                }
                else {
                    switch (enemyField[x, y]) {
                        case "collision":
                            enemyFieldString += _collisionCell;
                            break;
                        case "miss":
                            enemyFieldString += _missCell;
                            break;
                        default:
                            enemyFieldString += _fieldCell;
                            break;
                    }
                }
            }

            return enemyFieldString;
        }

        private string DrawPlayerField(string[,] playerField, int x) {
            string playerFieldString = "";
            playerFieldString += _defaultStyle + (x < 9 ? $" {x + 1} " : $"{x + 1} ") + Color.Reset;
            for (int y = 0; y < _fieldSize[0]; y++) {
                switch (playerField[x, y]) {
                    case null: case "border":
                        playerFieldString += _fieldCell;
                        break;
                    case "ship":
                        playerFieldString += _shipCell;
                        break;
                    case "collision":
                        playerFieldString += _collisionCell;
                        break;
                    case "miss":
                        playerFieldString += _missCell;
                        break;
                }
            }

            return playerFieldString;
        }

        private string DrawDefaultField(string[,] playerField, int x) {
            string fieldString = "";
            return fieldString;
        }
    }
}