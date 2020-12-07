using System;

namespace ConsoleApp {
    public class GameManager {
        private string[,] _playerOneField;
        private string[,] _playerTwoField;

        public void Start(string gameType, Settings settings) {
            switch (gameType) {
                case "Fast Game":
                    LoadFastGame(gameType, settings);
                    break;
                case "Player vs Player":
                    LoadPlayerVsPlayerGame(gameType, settings);
                    break;
                case "Player vs AI":
                    LoadPlayerVsAiGame(gameType, settings);
                    break;
                case "AI vs AI":
                    LoadAiVsAiGame(gameType, settings);
                    break;
            }
        }

        public void SetPlayerField(string[,] field) {
            if (_playerOneField == null) _playerOneField = field;
            else _playerTwoField = field;
        }

        private void LoadFastGame(string gameType, Settings settings) {
            _playerOneField = FieldManager.GenerateField(settings);
            _playerTwoField = FieldManager.GenerateField(settings);
            StartGame(gameType, settings, "Human", "AI");
        }

        private void LoadPlayerVsPlayerGame(string gameType, Settings settings) {
            string playerOneName = AskPlayerName(2, "first player");
            LoadBoardBuilder(settings, playerOneName);
            
            string playerTwoName = AskPlayerName(2, "second player");
            LoadBoardBuilder(settings, playerTwoName);

            if (playerOneName == playerTwoName) {
                playerOneName += "1"; 
                playerTwoName += "2";
            }
            StartGame(gameType, settings, playerOneName, playerTwoName);
        }

        private void LoadPlayerVsAiGame(string gameType, Settings settings) {
            string playerName = AskPlayerName();
            string aiName = AskAiName();
            
            LoadBoardBuilder(settings, playerName);
            _playerTwoField = FieldManager.GenerateField(settings);
            
            StartGame(gameType, settings, playerName, aiName);
        }

        private void LoadAiVsAiGame(string gameType, Settings settings) {
            _playerOneField = FieldManager.GenerateField(settings);
            _playerTwoField = FieldManager.GenerateField(settings);
            
            StartGame(gameType, settings,"Beavis", "Butthead");
        }

        private void LoadBoardBuilder(Settings settings, string playerName) {
            BoardRenderer boardRenderer = new BoardRenderer("Board Building", settings.BattlefieldSize,
                playerName) {SelectableFieldRowCount = settings.BattlefieldSize[1]};
            BoardBuilder boardBuilder = new BoardBuilder(this, boardRenderer, settings.BattlefieldSize);
            BuilderEventListener eventListener = new BuilderEventListener(boardRenderer, boardBuilder);
            boardBuilder.EventListener = eventListener;
            boardBuilder.Contact = settings.ShipArrangement;
            boardBuilder.Start(settings.ShipCount, settings.ShipSettings);
        }

        private void StartGame(string gameType, Settings settings, string playerOneName, string playerTwoName) {
            int shipsCapacity = FieldManager.CalculateShipsCapacity(settings.ShipCount, settings.ShipSettings);
            BoardRenderer gameRenderer = new BoardRenderer(gameType, settings.BattlefieldSize,
                playerOneName, playerTwoName) {SelectableFieldRowCount = settings.BattlefieldSize[1]};
            BattleManager battle = new BattleManager(gameRenderer, _playerOneField,
                _playerTwoField, playerOneName, playerTwoName, shipsCapacity) {GameMode = gameType};
            GameEventListener gameEventListener = new GameEventListener(battle, gameRenderer);
            battle.EventListener = gameEventListener;
            battle.Start();
        }

        private string AskPlayerName(int playersCount = 1, string currentPlayerNumber = "") {
            Console.Clear();
            Console.CursorVisible = true;
            if (playersCount == 1) Console.Write(Color.YellowText + "Please enter your name: " + Color.Reset);
            else Console.Write(Color.YellowText + $"Please enter {currentPlayerNumber} name: " + Color.Reset);
            string playerName = Console.ReadLine();
            if (playerName == null || playerName.Length <= 1) playerName = "Human";
            else if (playerName.Length >= 25) playerName = "Mr. Long Name";
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