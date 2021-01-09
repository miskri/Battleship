using System;
using System.Collections.Generic;
using ConsoleApp.Data;
using ConsoleApp.EventListeners;
using ConsoleApp.Graphics;
using ConsoleApp.Objects;
using ConsoleApp.Utils;

namespace ConsoleApp.Control {
    
    // Filling the GameProperties depending on the selected game mode
    public class GameManager {
       
        private GameProperties _props;

        public void Start(string gameType, Settings settings) {
            _props = new GameProperties(gameType);
            switch (gameType) {
                case "Fast Game":
                    LoadFastGame(settings, _props);
                    break;
                case "Player vs Player":
                    LoadPlayerVsPlayerGame(settings, _props);
                    break;
                case "Player vs AI":
                    LoadPlayerVsAiGame(settings, _props);
                    break;
                case "AI vs AI":
                    LoadAiVsAiGame(settings, _props);
                    break;
            }
        }

        public void SetPlayerField(string[,] field) {
            if (_props.Player1Field == null) {
                _props.Player1Field = field;
                _props.FieldSize = new[] {field.GetLength(0), field.GetLength(1)}; // TODO here may cause error with field size
                _props.LoadPlayer1FieldToArray();
            }
            else {
                _props.Player2Field = field;
                _props.LoadPlayer2FieldToArray();
            }
        }

        public void SetPlayerFlotilla(Flotilla flotilla) {
            if (_props.Player1Flotilla == null) _props.Player1Flotilla = flotilla;
            else _props.Player2Flotilla = flotilla;
        }

        private void LoadFastGame(Settings settings, GameProperties props) {
            props.Player1Name = "Human";
            props.Player2Name = "AI";
            
            (_props.Player1Field, _props.Player1Flotilla) = FieldManager.GenerateField(settings);
            (_props.Player2Field, _props.Player2Flotilla) = FieldManager.GenerateField(settings);
            
            _props.FieldSize = new[] {settings.BattlefieldSize[0], settings.BattlefieldSize[1]};
            StartGame(props);
        }

        public void LoadFastGameForWeb(GameProperties props) {
            props.Player1Name = "Human";
            props.Player2Name = "AI";
            Settings settings = new Settings {
                BattlefieldSize = new []{10, 10},
                ShipArrangement = false,
                ShipCount = new []{1, 1, 1, 1, 1},
                ShipNames = new []{"Carrier", "Battleship", "Submarine", "Cruiser", "Patrol"},
                ShipSettings = new []{5, 4, 3, 2, 1}
            };
            
            (props.Player1Field, props.Player1Flotilla) = FieldManager.GenerateField(settings);
            (props.Player2Field, props.Player2Flotilla) = FieldManager.GenerateField(settings);
            
            props.FieldSize = new[] {settings.BattlefieldSize[0], settings.BattlefieldSize[1]};
            
            props.BattleHistory = new List<string> {
                DataUtils.Default + $"{props.Player1Name} is ready!", DataUtils.Default + $"{props.Player2Name} is ready!"
            };
            props.CurrentPlayer = props.Player1Name;
            props.MenuOptions = new List<string>{"Main Menu", "Quit"};
            props.MenuOptions.Insert( 0, "Save");
            props.SelectableRowCount = props.FieldSize[0];
        }

        private void LoadPlayerVsPlayerGame(Settings settings, GameProperties props) {
            props.Player1Name = AskPlayerName(2, "first player");
            LoadBoardBuilder(settings, props, props.Player1Name);
            
            props.Player2Name = AskPlayerName(2, "second player");
            LoadBoardBuilder(settings, props, props.Player2Name);

            if (props.Player1Name == props.Player2Name) {
                props.Player1Name += "1"; 
                props.Player2Name += "2";
            }
            StartGame(props);
        }

        private void LoadPlayerVsAiGame(Settings settings, GameProperties props) {
            props.Player1Name = AskPlayerName();
            props.Player2Name = AskAiName();
            
            LoadBoardBuilder(settings, props, props.Player1Name);
            (_props.Player2Field, _props.Player2Flotilla) = FieldManager.GenerateField(settings);
            
            StartGame(props);
        }

        private void LoadAiVsAiGame(Settings settings, GameProperties props) {
            props.Player1Name = "Beavis";
            props.Player2Name = "Butthead";
            
            (_props.Player1Field, _props.Player1Flotilla) = FieldManager.GenerateField(settings);
            (_props.Player2Field, _props.Player2Flotilla) = FieldManager.GenerateField(settings);
            
            _props.FieldSize = new[] {settings.BattlefieldSize[0], settings.BattlefieldSize[1]};
            StartGame(props);
        }

        private void LoadBoardBuilder(Settings settings, GameProperties gameProps, string playerName) { 
            BuilderProperties builderProps = new BuilderProperties {
                Mode = "Board Building",
                PlayerFlotilla = playerName == gameProps.Player1Name ? gameProps.Player1Flotilla : gameProps.Player2Flotilla,
                PlayerField = new string[settings.BattlefieldSize[0], settings.BattlefieldSize[1]],
                ShipNames = settings.ShipNames,
                MenuOptions = new List<string>{"Main Menu", "Quit"},
                PlayerName = playerName,
                ShipArrangement = settings.ShipArrangement,
                SelectableRowCount = settings.BattlefieldSize[0],
                Builder = new BoardBuilder(this),
                Renderer = new BuilderRenderer(settings.BattlefieldSize[1]),
                EventListener = new BuilderEventListener()
            };
            builderProps.Builder.Start(settings, builderProps);
        }

        private void StartGame(GameProperties props) {
            props.Manager = new BattleManager();
            props.Renderer = new BoardRenderer(props.FieldSize[1]);
            props.EventListener = new BattleEventListener();
            props.Manager.StartBattle(props);
        }

        private string AskPlayerName(int playersCount = 1, string currentPlayerNumber = "") {
            Console.Clear();
            Console.CursorVisible = true;
            if (playersCount == 1) Console.Write(Color.YellowText + "Please enter your name: " + Color.Reset);
            else Console.Write(Color.YellowText + $"Please enter {currentPlayerNumber} name: " + Color.Reset);
            string playerName = Console.ReadLine();
            if (playerName == null || playerName.Length <= 1) playerName = "Human";
            else if (playerName.Length >= 15) playerName = "Mr. Long Name";
            Console.CursorVisible = false;
            return playerName;
        }

        private static string AskAiName() {
            string[] names = {"Deep Blue", "AlphaZero", "Pleo", "AIBO", "QRIO", "IBM Watson", "MYCIN", "20Q", "Wall-E", 
                "Digital Genius", "Skynet", "T1000", "Mart Calmo", "Yandex Alisa", "Apple Siri", "HAL 9000", "TARS", 
                "R2D2", "RoboCop", "Blue Brain Project", "AlphaGo", "iPavlov", "Donald Trump", "Matrix", "COVID-19"};
            return names[new Random().Next(0, names.Length)];
        }

    }
}