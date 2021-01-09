using System.Collections.Generic;
using ConsoleApp.Data;
using ConsoleApp.Utils;

namespace ConsoleApp.Graphics {
    public class BoardRenderer : Renderer {
        
        public BoardRenderer(int fieldWidth) {
            FieldWidth = fieldWidth;
            BottomAIndexes = GetBottomIndexes(FieldWidth);
            Separator = new string('-', FieldWidth * 3);
        }

        public void RenderBoard(GameProperties props, int row, int col) {
            if (props.Winner != null) {
                GameOverScreen(props, row);
                return;
            }
            string outputString = "";
            outputString += GetLevelDescription(props.GameMode, props.CurrentPlayer, props.Player1Name, props.Player2Name);
            outputString += GetFieldCoordinatesTemplate(props.GameMode);

            for (int i = 0; i < props.FieldSize[0]; i++) {
                outputString += GetField(props, row, col, i);
            }
            
            outputString += GetFieldFooter(props.GameMode);
            if (props.GameMode != "AI vs AI") {
                outputString += GetBattleInfo(props, row, col, props.CurrentPlayer);
            }
            
            outputString += GetBattleHistory(props.BattleHistory);
            if (props.GameMode != "AI vs AI") outputString += GetBattleMenu(props.MenuOptions, row, 
                props.FieldSize[0]);

            RenderScreen(outputString);
        }

        public void GameOverScreen(GameProperties props, int row) {
            string outputString = "";
            outputString += GetLevelDescription(props.GameMode, props.CurrentPlayer, props.Player1Name, props.Player2Name);
            outputString += GetFieldCoordinatesTemplate(props.GameMode);
            
            for (int i = 0; i < props.FieldSize[0]; i++) {
                outputString += GetFieldsAfterGameEnding(props, i);
            }
            
            outputString += GetFieldFooter(props.GameMode);
            outputString += GetBattleHistory(props.BattleHistory);
            outputString += DataUtils.Default + $"   {props.Winner} wins the game! Congratulations!" + Color.Reset + "\n";
            outputString += DataUtils.Default + $"   {Separator}" + Color.Reset + "\n";
            outputString += GetBattleMenu(props.MenuOptions, row, props.SelectableRowCount);

            RenderScreen(outputString);
        }

        private string GetFieldsAfterGameEnding(GameProperties props, int index) {
            string fieldsAfterGame = "";
            if (props.GameMode == "AI vs AI" || props.GameMode == "Player vs Player" && props.Winner == props.Player2Name) {
                fieldsAfterGame += DrawPlayerField(props.Player1Field, index);
                fieldsAfterGame += DataUtils.Default + "      " + Color.Reset;
                fieldsAfterGame += DrawPlayerField(props.Player2Field, index) + "\n";
            }
            else {
                fieldsAfterGame += DrawPlayerField(props.Player2Field, index);
                fieldsAfterGame += DataUtils.Default + "      " + Color.Reset;
                fieldsAfterGame += DrawPlayerField(props.Player1Field, index) + "\n";
            }

            return fieldsAfterGame;
        }

        private string GetField(GameProperties props, int row, int col, int index) { 
            string fieldString = "";
            if (props.GameMode != "AI vs AI" && props.GameMode != "Player vs Player") {
                fieldString += DrawEnemyField(props.Player2Field, row, col, index);
                fieldString += DataUtils.Default + "      " + Color.Reset;
                fieldString += DrawPlayerField(props.Player1Field, index);
            }
            else if (props.GameMode == "Player vs Player") {
                fieldString += DrawEnemyField(props.GetDefenderField(), row, col, index);
                fieldString += DataUtils.Default + "      " + Color.Reset;
                fieldString += DrawPlayerField(props.GetAttackerField(), index);
            }
            else {
                fieldString += DrawPlayerField(props.Player1Field, index);
                fieldString += DataUtils.Default + "      " + Color.Reset;
                fieldString += DrawPlayerField(props.Player2Field, index);
            }
            fieldString += "\n";
            return fieldString;
        }

        private string GetBattleInfo(GameProperties props, int row, int col, string currentPlayer) {
            string battleInfoString = "";
            battleInfoString += DataUtils.Default + "   Selected quad: ";
            if (row >= 0 && col >= 0 && row < props.FieldSize[0] && col < props.FieldSize[1]) {
                battleInfoString += DataUtils.Alphabet[col] + $"{row + 1}";
            }

            battleInfoString += "\n";
            battleInfoString += DataUtils.Default + $"   This is {currentPlayer} turn! \n";
            battleInfoString += DataUtils.Default + $"   Ships in enemy flotilla: {props.GetEnemyShipsCount()}\n";
            
            if (props.GameMode != "Player vs Player" && props.Manager.TimeManager.GetStepsBackCount(props.Id) > 0) 
                battleInfoString += DataUtils.Default + "   Сharges in the time machine: " +
                                    $"{props.Manager.TimeManager.GetStepsBackCount(props.Id)}\n";
            
            battleInfoString += $"   {Separator} \n";
            return battleInfoString;
        }

        private string GetBattleHistory(List<string> battleHistory) {
            string battleHistoryString = "";
            battleHistoryString += "   Battle History: \n";
            battleHistoryString += $"   {battleHistory[^2]} \n";
            battleHistoryString += $"   {battleHistory[^1]} \n";
            battleHistoryString += $"   {Separator}" + Color.Reset + "\n";
            return battleHistoryString;
        }
        
        private string DrawEnemyField(string[,] enemyField, int row, int col, int index) {
            string enemyFieldString = "";
            enemyFieldString += DataUtils.Default + (index < 9 ? $" {index + 1} " : $"{index + 1} ") + Color.Reset;
            for (int x = 0; x < enemyField.GetLength(1); x++) {
                if (index == row && x == col) {
                    enemyFieldString += enemyField[index, x] switch {
                        "collision" => (Color.WhiteBackground + Color.BlackText + " X " + Color.Reset),
                        "miss" => (Color.WhiteBackground + Color.BlackText + " ♦ " + Color.Reset),
                        _ => (Color.WhiteBackground + "   " + Color.Reset)
                    };
                }
                else {
                    enemyFieldString += enemyField[index, x] switch {
                        "collision" => DataUtils.CollisionCell,
                        "miss" => DataUtils.MissCell,
                        _ => DataUtils.FieldCell
                    };
                }
            }

            return enemyFieldString;
        }

        private string DrawPlayerField(string[,] playerField, int index) {
            string playerFieldString = "";
            playerFieldString += DataUtils.Default + (index < 9 ? $" {index + 1} " : $"{index + 1} ") + Color.Reset;
            for (int x = 0; x < playerField.GetLength(1); x++) {
                switch (playerField[index, x]) {
                    case null: case "border":
                        playerFieldString += DataUtils.FieldCell;
                        break;
                    case "ship":
                        playerFieldString += DataUtils.ShipCell;
                        break;
                    case "collision":
                        playerFieldString += DataUtils.CollisionCell;
                        break;
                    case "miss":
                        playerFieldString += DataUtils.MissCell;
                        break;
                }
            }

            return playerFieldString;
        }
    }
}