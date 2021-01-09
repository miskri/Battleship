using System;
using System.Collections.Generic;
using ConsoleApp.Control;
using ConsoleApp.EventListeners;
using ConsoleApp.Graphics;
using ConsoleApp.Objects;

namespace ConsoleApp.Data {
    
    // Set of necessary data for transfer between BattleManager, BoardRenderer, BattleEventListener
    [Serializable] public class GameProperties {

        public string Id { get; set; }
        public string GameMode { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public Flotilla Player1Flotilla { get; set; }
        public Flotilla Player2Flotilla { get; set; }
        [NonSerialized] public string[,] Player1Field;
        [NonSerialized] public string[,] Player2Field;
        public int[] FieldSize { get; set; }
        public string[] Player1FieldArray { get; set; } = {};
        public string[] Player2FieldArray { get; set; } = {};
        public string CurrentPlayer { get; set; }
        public int Round { get; set; }
        public int SelectableRowCount { get; set; }
        [NonSerialized] public string Winner;
        public List<string> BattleHistory { get; set; }
        public List<string> MenuOptions { get; set; }
        [NonSerialized] public BattleManager Manager;
        [NonSerialized] public BoardRenderer Renderer;
        [NonSerialized] public BattleEventListener EventListener;
        
        public GameProperties() {}
        public GameProperties(string gameMode) {
            GameMode = gameMode;
            string gameId = $"{GameMode} - {DateTime.Now}";
            Id = gameId.GetHashCode().ToString();
        }
        
        public string[,] GetAttackerField() {
            return CurrentPlayer == Player1Name ? Player1Field : Player2Field;
        }
        
        public string[,] GetDefenderField() {
            return CurrentPlayer == Player1Name ? Player2Field : Player1Field;
        }

        public string GetDefenderName() {
            return CurrentPlayer == Player1Name ? Player2Name : Player1Name;
        }

        public Flotilla GetAttackedFlotilla() {
            return CurrentPlayer == Player1Name ? Player2Flotilla : Player1Flotilla;
        }

        public void SwitchPlayerTurn() {
            CurrentPlayer = CurrentPlayer == Player1Name ? Player2Name : Player1Name;
        }

        public int GetEnemyShipsCount() {
            return CurrentPlayer == Player1Name ? Player2Flotilla.Ships.Count : Player1Flotilla.Ships.Count;
        }

        public void LoadPlayer1FieldToArray() {
            Player1FieldArray = new string[Player1Field.Length];
            FieldToArray(Player1FieldArray, Player1Field);
        }

        public void LoadPlayer2FieldToArray() {
            Player2FieldArray = new string[Player2Field.Length];
            FieldToArray(Player2FieldArray, Player2Field);
        }
        
        public void LoadPlayer1FieldFromArray() {
            Player1Field = new string[FieldSize[0], FieldSize[1]];
            FieldFromArray(Player1FieldArray, Player1Field);
        }
        
        public void LoadPlayer2FieldFromArray() {
            Player2Field = new string[FieldSize[0], FieldSize[1]];
            FieldFromArray(Player2FieldArray, Player2Field);
        }

        private void FieldToArray(string[] fieldArray, string[,] field) {
            int index = 0;
            for (int i = 0; i < field.GetLength(0); i++) {
                for (int j = 0; j < field.GetLength(1); j++) {
                    fieldArray[index++] = field[i, j];
                }
            }
        }

        private void FieldFromArray(string[] fieldArray, string[,] field) {
            for (int i = 0; i < fieldArray.Length; i++) {
                int row = i / FieldSize[1];
                int col = i - row * FieldSize[1];
                field[row, col] = fieldArray[i];
            }
        }
    }
}