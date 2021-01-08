using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp {
    public class BattleManager {

        public readonly TimeTravellingManager TimeManager = new TimeTravellingManager(); 
        private readonly Random _rand = new Random();
        
        // Load battle history, menu options, define current player and start EventListener processing.
        public void StartBattle(GameProperties props) {
            props.BattleHistory = new List<string> {
                Data.Default + $"{props.Player1Name} is ready!", Data.Default + $"{props.Player2Name} is ready!"
            };
            props.CurrentPlayer = props.Player1Name;
            props.MenuOptions = new List<string>{"Main Menu", "Quit"};
            props.MenuOptions.Insert( 0, "Save");
            
            if (props.GameMode != "AI vs AI") {
                props.SelectableRowCount = props.FieldSize[0];
                props.Renderer.RenderBoard(props, 0, 0);
            } else {
                RunAiBattle(props);
            }
            props.EventListener.EventListener(props, 0, 0);
        }

        // Assign Manager, Renderer and EventListener components, load players fields and ships, start EventListener.
        public void LoadBattle(GameProperties props) {
            if (props.Manager == null) props.Manager = this;
            props.Renderer = new BoardRenderer(props.FieldSize[1]);
            props.EventListener = new BattleEventListener();
            props.Player1Flotilla.LoadShipFromArrays();
            props.Player2Flotilla.LoadShipFromArrays();
            props.LoadPlayer1FieldFromArray();
            props.LoadPlayer2FieldFromArray();

            if (props.Round < 1 || TimeManager.GetStepsBackCount(props.Id) == 0) props.MenuOptions.Remove("Step back");
            
            props.Renderer.RenderBoard(props, props.SelectableRowCount + 1, 0);
            props.EventListener.EventListener(props, props.SelectableRowCount + 1, 0);
        }

        // Checking that a potential move does not repeat an already made move.
        public bool CheckMove(string[,] fieldToCheck, int row, int col) {
            return fieldToCheck[row, col] != "collision" && fieldToCheck[row, col] != "miss";
        }
        
        // Register hit, switch turn, render changes
        public void MakeMove(GameProperties props, int row, int col) {
            props.Round++;
            switch (props.GameMode) {
                case "Player vs Player":
                    RegisterHit(props, row, col);
                    Thread.Sleep(2000); // pause for player switching
                    props.SwitchPlayerTurn();
                    props.Renderer.RenderBoard(props, row, col);
                    break;
                
                default: // game mode is "Player vs AI"
                    if (TimeManager.GetStepsBackCount(props.Id) > 0) WritePreviousStep(props); // remember previous step

                    RegisterHit(props, row, col);
                    if (props.Winner != null) break;
                    props.Round++;
                    props.SwitchPlayerTurn();

                    props.Player1Field = AiMove(props, props.Player1Field);
                    props.SwitchPlayerTurn();
                    
                    props.Renderer.RenderBoard(props, row, col);
                    break;
            }
        }

        // Register damage if the move made hit the ship, indicate a miss if not
        private void RegisterHit(GameProperties props, int row, int col) {
            if (props.GetDefenderField()[row, col] == "ship") {
                props.GetDefenderField()[row, col] = "collision";
                AddToBattleHistory(props, row, col, Data.BattleMessageHit);
                props.GetAttackedFlotilla().SetDamage(props, row, col);
                CheckPlayerFlotillas(props, props.CurrentPlayer);
            }
            else {
                props.GetDefenderField()[row, col] = "miss";
                AddToBattleHistory(props, row, col, Data.BattleMessageMiss);
            }
        }

        // If one of the fleets is destroyed - name the winner and render game over screen
        private void CheckPlayerFlotillas(GameProperties props, string attackingSide) {
            if (!props.GetAttackedFlotilla().Destroyed) return;
            
            props.Winner = attackingSide;
            props.MenuOptions.Remove("Save");
            props.MenuOptions.Remove("Step back");
            props.BattleHistory.Add($"{Color.YellowText}{props.Winner} destroys all enemy ships!");
            props.SelectableRowCount = 0;
            props.Renderer.GameOverScreen(props, 0);
        }

        // AI move, now it just generates random shot
        private string[,] AiMove(GameProperties props, string[,] field) {
            bool moveIsDone = false;
            while (!moveIsDone) {
                int row = _rand.Next(0, field.GetLength(0));
                int col = _rand.Next(0, field.GetLength(1));
                switch (field[row, col]) {
                    case "ship":
                        field[row, col] = "collision";
                        AddToBattleHistory(props, row, col, Data.BattleMessageHit);
                        props.GetAttackedFlotilla().SetDamage(props, row, col);
                        CheckPlayerFlotillas(props, props.CurrentPlayer);
                        moveIsDone = true;
                        break;
                    case null: case "border":
                        field[row, col] = "miss";
                        AddToBattleHistory(props, row, col, Data.BattleMessageMiss);
                        moveIsDone = true;
                        break;
                }
            }
            return field;
        }
        
        // Starting an AI duel, drawing each move with a pause of 1 second until one of the AI wins
        private void RunAiBattle(GameProperties props) {
            int roundsCount = props.Player1Field.GetLength(0) * props.Player1Field.GetLength(1) * 2;
            for (int rounds = 1; rounds <= roundsCount; rounds++) {
                if (props.Winner != null) break;
                props.Renderer.RenderBoard(props, 0, 0);
                Thread.Sleep(1000);
                props.Round++;
                if (rounds % 2 != 0) {
                    props.Player2Field = AiMove(props, props.Player2Field);
                }
                else {
                    props.Player1Field = AiMove(props, props.Player1Field);
                }
                props.SwitchPlayerTurn();
            }
        }

        // Add message or hit result in battle history
        public void AddToBattleHistory(GameProperties props, int row = -1, int col = -1, string result = "", string message="") {
            props.BattleHistory.Add(result != ""
                ? $"{Color.YellowText}Round {props.Round} - {props.CurrentPlayer} hit {Data.Alphabet[col]}{row + 1}." +
                  $" Result - {result}{Color.YellowText}!"
                : message);
        }

        // Write previous step using TimeManager
        private void WritePreviousStep(GameProperties props) {
            if (!props.MenuOptions.Contains("Step back")) props.MenuOptions.Insert(0, "Step back");
            TimeManager.WritePreviousStep(props);
        }
        
        // Load previous GameProperties from TimeManager
        public void DoStepBack(GameProperties props) {
            GameProperties newProps = TimeManager.GetPreviousStep(props);
            LoadBattle(newProps); // Load it if game runs in Console
        }

        public void SaveProgress(GameProperties props) {
            string saveName = $"{props.GameMode} - {DateTime.Now}"; // save name template: Game mode - saving time
            if (SaveManager.Saves == null) SaveManager.LoadSaves();
            
            if (props.Round > 1 && !SaveManager.ContainsThisName(saveName)) {
                props.LoadPlayer1FieldToArray(); // convert 2D arrays to 1D array for saving
                props.LoadPlayer2FieldToArray();

                GameProperties save = props.DeepClone(); // Data.DeepClone(obj)
                if (props.GameMode == "Player vs AI" || props.GameMode == "Fast Game") {
                    TimeManager.SaveRoundHistory(save.Id); // save round history if its Player vs AI mode
                }
                
                SaveManager.SaveGame(new Save(saveName, save)); 
                AddToBattleHistory(props, message: $"{Color.YellowText}Game was saved successfully!{Color.Reset}");
            }
            else if (props.Round == 0) {
                AddToBattleHistory(props, message: $"{Color.RedText}You can't save game on first round!{Color.Reset}");
            }
            else {
                AddToBattleHistory(props, message: $"{Color.RedText}Your game already saved!{Color.Reset}");
            }
        }
    }
}