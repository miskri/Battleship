using System;
using System.Collections.Generic;
using ConsoleApp.Control;
using ConsoleApp.Data;
using ConsoleApp.Utils;

namespace ConsoleApp.Graphics {
    public class BuilderRenderer : Renderer {
        
        public string[,] TemporaryField;
        public BuilderRenderer(int fieldWidth) {
            FieldWidth = fieldWidth;
            BottomAIndexes = GetBottomIndexes(FieldWidth);
            Separator = new string('-', FieldWidth * 3);
        }
        
        public void RenderBoard(BuilderProperties props, int row, int col) {
            string outputString = "";
            outputString += GetLevelDescription(props.Mode, props.PlayerName);
            outputString += GetFieldCoordinatesTemplate(props.Mode);
            outputString += DrawDefaultField(props, row, col);
            outputString += GetFieldFooter(props.Mode);
            outputString += GetBoardBuildingInfo(props, props.CursorLength);
            outputString += GetBattleMenu(props.MenuOptions, row, props.SelectableRowCount);

            RenderScreen(outputString);
        }
        
        private string DrawDefaultField(BuilderProperties props, int row, int col) {
            string fieldString = "";
            string[,] fieldTemplate = GetFieldTemplate(props, row, col, props.CursorLength, props.Direction);
            for (int i = 0; i < props.PlayerField.GetLength(0); i++) {
                fieldString += DataUtils.Default + (i < 9 ? $" {i + 1} " : $"{i + 1} ") + Color.Reset;
                for (int j = 0; j < props.PlayerField.GetLength(1); j++) {
                    switch (fieldTemplate[i, j]) {
                        case null: 
                            fieldString += DataUtils.FieldCell;
                            break;
                        case "st true":
                            fieldString += DataUtils.ValidTemplateCell;
                            break;
                        case "st false":
                            fieldString += DataUtils.InvalidTemplateCell;
                            break;
                        case "border":
                            fieldString += DataUtils.BorderCell;
                            break;
                        case "ship":
                            fieldString += DataUtils.ShipCell;
                            break;
                    }
                }
                fieldString += "\n";
            }

            TemporaryField = fieldTemplate;
            return fieldString;
        }
        
        private string[,] GetFieldTemplate(BuilderProperties props, int row, int col, int cursorLength, int direction) {
            string[,] currentField = new string[props.PlayerField.GetLength(0), props.PlayerField.GetLength(1)]; 
            Array.Copy(props.PlayerField, currentField, props.PlayerField.Length);
            int height = props.PlayerField.GetLength(0);
            int weight = props.PlayerField.GetLength(1);
            if (row >= height) {
                return currentField;
            }

            string shipTemplateName;
            List<string> availableDir = FieldManager.GetAvailableDirections(props.ShipArrangement, currentField, row, col, cursorLength);
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

            return currentField;
        }
        
        private string GetBoardBuildingInfo(BuilderProperties props, int shipSize) {
            string boardBuildingInfo = "";
            boardBuildingInfo += DataUtils.Default + $"   Current ship: {props.CurrentShipName} \n";
            boardBuildingInfo += $"   {Separator} \n";
            boardBuildingInfo += "   Press R to rotate your ship \n";
            boardBuildingInfo += $"   {Separator}" + Color.Reset + "\n";
            return boardBuildingInfo;
        }
    }
}