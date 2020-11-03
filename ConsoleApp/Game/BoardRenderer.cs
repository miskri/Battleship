using System;
using System.Collections.Generic;

namespace ConsoleApp {
    public class BoardRenderer {
        public readonly char[] Alphabet = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public List<string> BattleHistory = new List<string>();
        public List<string> MenuOptions = new List<string> {"Main Menu", "Quit"};
        public readonly string GameMode;
        public string Winner;
        private readonly string _playerOneName, _playerTwoName;
        private string _bottomAIndexes, _separator;
        public string[,] TemporaryField;
        public int SelectableFieldRowCount = 0;
        public readonly int[] FieldSize;

        private readonly string _defaultStyle = Color.BlackBackground + Color.YellowText;
        private readonly string _systemStyle = Color.BlackBackground + Color.GreenText;
        private readonly string _fieldCell = Color.BlueBackground + "   " + Color.Reset;
        private readonly string _shipCell = Color.GrayDarkBackground + "   " + Color.Reset;
        private readonly string _shipBorderCell = Color.GrayLightBackground + "   " + Color.Reset;
        private readonly string _shipTemplateTrueCell = Color.GreenLightBackground + "   " + Color.Reset;
        private readonly string _shipTemplateFalseCell = Color.RedLightBackground + "   " + Color.Reset;
        private readonly string _collisionCell = Color.RedBackground + Color.BlackText + " X " + Color.Reset;
        private readonly string _missCell = Color.BlueLightBackground + Color.BlackText + " ♦ " + Color.Reset;
        private readonly string _emptySpace = new string(' ', 9);

        public BoardRenderer(string gameMode, int[] fieldSize, string playerOneName = null,
            string playerTwoName = null) {
            GameMode = gameMode;
            FieldSize = fieldSize;
            _bottomAIndexes = GetBottomIndexes(fieldSize[0]);
            _separator = new string('-', fieldSize[0] * 3);

            _playerOneName = playerOneName;
            _playerTwoName = playerTwoName;
            
            BattleHistory.Add($"{playerTwoName} is ready!");
            BattleHistory.Add($"{playerOneName} is ready!");
            
            if (GameMode != "Board Building" && GameMode != "AI vs AI") MenuOptions.Insert(0, "Save Game");
        }

        private string GetBottomIndexes(int size) {
            string str = "";
            for (int i = 0; i < size; i++) {
                str += $" {Alphabet[i]} ";
            }

            return str;
        }

        public void RenderBoard(string[,] playerField, string[,] enemyField, int row, int col, string currentPlayer,
            GameEventListener eventListener = null) {
            if (Winner != null) GameOverScreen(eventListener, playerField, enemyField, row, col);
            Console.Clear();
            string outputString = "";
            outputString += GetLevelDescription();
            outputString += GetFieldCoordinatesTemplate();

            for (int i = 0; i < FieldSize[1]; i++) {
                outputString += GetField(playerField, enemyField, row, col, i);
            }
            
            outputString += GetFieldFooter();
            if (GameMode != "AI vs AI") {
                outputString += GetBattleInfo(row, col, currentPlayer);
            }
            
            outputString += GetBattleHistory();
            if (GameMode != "AI vs AI") outputString += GetBattleMenu(row);

            Console.WriteLine(outputString);
            eventListener?.EventListener(playerField, enemyField, row, col, currentPlayer, 
                SelectableFieldRowCount);
        }

        public void GameOverScreen(GameEventListener eventListener, string[,] playerField, string[,] enemyField, 
            int row, int col) {
            Console.Clear();
            string outputString = "";
            SelectableFieldRowCount = 0;
            outputString += GetLevelDescription();
            outputString += GetFieldCoordinatesTemplate();
            
            for (int i = 0; i < FieldSize[1]; i++) {
                outputString += GetFieldsAfterGameEnding(playerField, enemyField, i);
            }
            
            outputString += GetFieldFooter();
            outputString += GetBattleHistory();
            outputString += _defaultStyle + $"   {Winner} wins the game! Congratulations!" + Color.Reset + "\n";
            outputString += _defaultStyle + $"   {_separator}" + Color.Reset + "\n";
            outputString += GetBattleMenu(row);

            Console.WriteLine(outputString);
            eventListener.EventListener(playerField, enemyField, row, col, "", 
                SelectableFieldRowCount);
        }
        
        public void RenderBoardBuilder(BuilderEventListener eventListener, string[,] playerField, 
            int row, int col, int cursorLength, int direction, bool contact) {
            Console.Clear();
            string outputString = "";
            outputString += GetLevelDescription();
            outputString += GetFieldCoordinatesTemplate();
            outputString += DrawDefaultField(playerField, row, col, cursorLength, direction, contact);
            outputString += GetFieldFooter();
            outputString += GetBoardBuildingInfo(cursorLength);
            outputString += GetBattleMenu(row);

            Console.WriteLine(outputString);
            eventListener.EventListener(playerField, row, col, cursorLength, direction, SelectableFieldRowCount);
        }

        private string GetLevelDescription() {
            if (GameMode != "Board Building") {
                return _systemStyle + $"   Game mode: {GameMode}, {_playerOneName} against {_playerTwoName}" +
                       Color.Reset + "\n";
            }

            return _systemStyle + $"   {_playerOneName}, please make up the battle formation for your ships" + Color.Reset + "\n";
        }

        private string GetFieldCoordinatesTemplate() {
            string template = "";
            if (GameMode != "Board Building") {
                template += _defaultStyle + $"   {_separator}{_emptySpace}{_separator} \n";
                template += $"   {_bottomAIndexes}{_emptySpace}{_bottomAIndexes} \n";
            }
            else {
                template += _defaultStyle + $"   {_separator}\n";
                template += $"   {_bottomAIndexes} \n";
            }

            return template;
        }

        private string GetFieldsAfterGameEnding(string[,] playerField, string[,] enemyField, int index) {
            string fieldsAfterGame = "";
            if (GameMode == "AI vs AI" || GameMode == "Player vs Player" && Winner == _playerTwoName) {
                fieldsAfterGame += DrawPlayerField(playerField, index);
                fieldsAfterGame += _defaultStyle + "      " + Color.Reset;
                fieldsAfterGame += DrawPlayerField(enemyField, index) + "\n";
            }
            else {
                fieldsAfterGame += DrawPlayerField(enemyField, index);
                fieldsAfterGame += _defaultStyle + "      " + Color.Reset;
                fieldsAfterGame += DrawPlayerField(playerField, index) + "\n";
            }

            return fieldsAfterGame;
        }

        private string GetField(string[,] playerField, string[,] enemyField, int row, int col, int index) {
            string fieldString = "";
            if (GameMode != "Board Building" && GameMode != "AI vs AI") {
                fieldString += DrawEnemyField(enemyField, row, col, index);
                fieldString += _defaultStyle + "      " + Color.Reset;
                fieldString += DrawPlayerField(playerField, index);
            }
            else if (GameMode == "AI vs AI") {
                fieldString += DrawPlayerField(playerField, index);
                fieldString += _defaultStyle + "      " + Color.Reset;
                fieldString += DrawPlayerField(enemyField, index);
            }
            fieldString += "\n";
            return fieldString;
        }

        private string GetFieldFooter() {
            string fieldFooterString = "";
            if (GameMode != "Board Building" && GameMode != "AI vs AI") {
                fieldFooterString += _defaultStyle + $"   Enemy field{new string(' ', _separator.Length - 2)}Your field \n";
                fieldFooterString += $"   {_separator}{_emptySpace}{_separator} \n";
            }
            else if (GameMode == "AI vs AI") {
                fieldFooterString += _defaultStyle + $"   Beavis field{new string(' ', _separator.Length - 3)}Butthead field \n";
                fieldFooterString += $"   {_separator}{_emptySpace}{_separator} \n";
            }
            else {
                fieldFooterString += _defaultStyle + $"   {_separator}" + Color.Reset + "\n";
            }
            
            return fieldFooterString;
        }

        private string GetBoardBuildingInfo(int shipSize) {
            string boardBuildingInfo = "";
            string shipType = "";
            switch (shipSize) {
                case 5:
                    shipType = "Carrier";
                    break;
                case 4:
                    shipType = "Battleship";
                    break;
                case 3:
                    shipType = "Submarine";
                    break;
                case 2:
                    shipType = "Cruiser";
                    break;
                case 1:
                    shipType = "Patrol";
                    break;
            }
            boardBuildingInfo += _defaultStyle + $"   Current ship: {shipType} \n";
            boardBuildingInfo += $"   {_separator} \n";
            boardBuildingInfo += "   Press R to rotate your ship \n";
            boardBuildingInfo += $"   {_separator}" + Color.Reset + "\n";
            return boardBuildingInfo;
        }

        private string GetBattleInfo(int row, int col, string currentPlayer) {
            string battleInfoString = "";
            battleInfoString += "   Selected quad: ";
            if (col >= 0 && row >= 0 && col < FieldSize[0] && row < FieldSize[1]) {
                battleInfoString += Alphabet[col] + $"{row + 1}";
            }

            battleInfoString += "\n";
            battleInfoString += $"   This is {currentPlayer} turn! \n";
            battleInfoString += $"   {_separator} \n";
            return battleInfoString;
        }

        private string GetBattleHistory() {
            string battleHistory = "";
            battleHistory += "   Battle History: \n";
            battleHistory += $"   {BattleHistory[^2]} \n";
            battleHistory += $"   {BattleHistory[^1]} \n";
            battleHistory += $"   {_separator}" + Color.Reset + "\n";
            return battleHistory;
        }

        private string GetBattleMenu(int row) {
            string battleMenu = "";
            for (int z = 0; z < MenuOptions.Count; z++) {
                battleMenu += Color.BlackBackground + "  " + Color.Reset;
                if (row - SelectableFieldRowCount == z) {
                    battleMenu += Color.BlueBackground + Color.WhiteText +
                                  $" {MenuOptions[z]}{new string(' ', (FieldSize[0] * 3) - MenuOptions[z].Length)}" +
                                  Color.Reset + "\n";
                    continue;
                }

                battleMenu += _systemStyle + $" {MenuOptions[z]}" + Color.Reset + "\n";
            }

            return battleMenu;
        }

        private string DrawEnemyField(string[,] enemyField, int row, int col, int index) {
            string enemyFieldString = "";
            enemyFieldString += _defaultStyle + (index < 9 ? $" {index + 1} " : $"{index + 1} ") + Color.Reset;
            for (int y = 0; y < FieldSize[0]; y++) {
                if (index == row && y == col) {
                    switch (enemyField[index, y]) {
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
                    switch (enemyField[index, y]) {
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

        private string DrawPlayerField(string[,] playerField, int index) {
            string playerFieldString = "";
            playerFieldString += _defaultStyle + (index < 9 ? $" {index + 1} " : $"{index + 1} ") + Color.Reset;
            for (int y = 0; y < FieldSize[0]; y++) {
                switch (playerField[index, y]) {
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

        private string DrawDefaultField(string[,] playerField, int row, int col, int cursorLength, int direction, bool contact) {
            string fieldString = "";
            string[,] fieldTemplate = GetFieldTemplate(contact, playerField, row, col, cursorLength, direction);
            for (int i = 0; i < FieldSize[1]; i++) {
                fieldString += _defaultStyle + (i < 9 ? $" {i + 1} " : $"{i + 1} ") + Color.Reset;
                for (int j = 0; j < FieldSize[0]; j++) {
                    switch (fieldTemplate[i, j]) {
                        case null: 
                            fieldString += _fieldCell;
                            break;
                        case "st true":
                            fieldString += _shipTemplateTrueCell;
                            break;
                        case "st false":
                            fieldString += _shipTemplateFalseCell;
                            break;
                        case "border":
                            fieldString += _shipBorderCell;
                            break;
                        case "ship":
                            fieldString += _shipCell;
                            break;
                    }
                }
                fieldString += "\n";
            }

            TemporaryField = fieldTemplate;
            return fieldString;
        }

        private string[,] GetFieldTemplate(bool contact, string[,] playerField, int row, int col, int cursorLength, int direction) {
            string[,] currentField = new string[FieldSize[0], FieldSize[1]]; 
            Array.Copy(playerField, currentField, playerField.Length);
            int weight = FieldSize[0];
            int height = FieldSize[1];
            if (row < height) {
                string shipTemplateName;
                List<string> availableDir = FieldManager.CheckPosition(contact, currentField, row, col, cursorLength);
                if (direction == 0 && availableDir.Contains("down") || direction == 1 && availableDir.Contains("right")) {
                    shipTemplateName = "st true";
                }
                else shipTemplateName = "st false";
                for (int a = 0; a < cursorLength; a++) {
                    if (direction == 0 && col + a < weight && row < height) {
                        currentField[row, col + a] = shipTemplateName;
                    }
                    else if (direction == 1 && row + a < height && col < weight) {
                        currentField[row + a, col] = shipTemplateName;
                    }
                }
            }

            return currentField;
        }
    }
}