using System;

namespace ConsoleApp {
    public class GameManager {
        private string[,] _playerOneField;
        private string[,] _playerTwoField;

        public void Start(string gameType, bool contact, int[] shipCount, int[] shipSettings, int[] battlefieldSize) {
            switch (gameType) {
                case "Fast Game":
                    LoadFastGame(gameType, contact, shipCount, shipSettings, battlefieldSize);
                    break;
                case "Player vs Player":
                    LoadPlayerVsPlayerGame(gameType, contact, shipCount, shipSettings, battlefieldSize);
                    break;
                case "Player vs AI":
                    LoadPlayerVsAiGame(gameType, contact, shipCount, shipSettings, battlefieldSize);
                    break;
                case "AI vs AI":
                    LoadAiVsAiGame(gameType, contact, shipCount, shipSettings, battlefieldSize);
                    break;
            }
        }

        public void SetPlayerField(string[,] field) {
            if (_playerOneField == null) _playerOneField = field;
            else _playerTwoField = field;
        }

        private void LoadFastGame(string gameType, bool contact, int[] shipCount, int[] shipSettings, 
            int[] battlefieldSize) {
            _playerOneField = FieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
            _playerTwoField = FieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
            StartGame(gameType, shipCount, shipSettings, battlefieldSize, "Human", "AI");
        }

        private void LoadPlayerVsPlayerGame(string gameType, bool contact, int[] shipCount, int[] shipSettings,
            int[] battlefieldSize) {
            string playerOneName = AskPlayerName(2, "first player");
            LoadBoardBuilder(contact, shipCount, shipSettings, battlefieldSize, playerOneName);
            
            string playerTwoName = AskPlayerName(2, "second player");
            LoadBoardBuilder(contact, shipCount, shipSettings, battlefieldSize, playerTwoName);

            if (playerOneName == playerTwoName) {
                playerOneName += "1"; 
                playerTwoName += "2";
            }
            StartGame(gameType, shipCount, shipSettings, battlefieldSize, playerOneName, playerTwoName);
        }

        private void LoadPlayerVsAiGame(string gameType, bool contact, int[] shipCount, int[] shipSettings, 
            int[] battlefieldSize) {
            string playerName = AskPlayerName();
            string aiName = AskAiName();
            
            LoadBoardBuilder(contact, shipCount, shipSettings, battlefieldSize, playerName);
            _playerTwoField = FieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
            
            StartGame(gameType, shipCount, shipSettings, battlefieldSize, playerName, aiName);
        }

        private void LoadAiVsAiGame(string gameType, bool contact, int[] shipCount, int[] shipSettings, 
            int[] battlefieldSize) {
            _playerOneField = FieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
            _playerTwoField = FieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
            
            StartGame(gameType, shipCount, shipSettings, battlefieldSize,"Beavis", "Butthead");
        }

        private void LoadBoardBuilder(bool contact, int[] shipCount, int[] shipSettings, int[] battlefieldSize, 
            string playerName) {
            BoardRenderer boardRenderer = new BoardRenderer("Board Building", battlefieldSize,
                playerName) {SelectableFieldRowCount = battlefieldSize[1]};
            BoardBuilder boardBuilder = new BoardBuilder(this, boardRenderer, battlefieldSize);
            BuilderEventListener eventListener = new BuilderEventListener(boardRenderer, boardBuilder);
            boardBuilder.EventListener = eventListener;
            boardBuilder.Contact = contact;
            boardBuilder.Start(shipCount, shipSettings);
        }

        private void StartGame(string gameType, int[] shipCount, int[] shipSettings, int[] battlefieldSize, 
            string playerOneName, string playerTwoName) {
            int shipsCapacity = FieldManager.CalculateShipsCapacity(shipCount, shipSettings);
            BoardRenderer gameRenderer = new BoardRenderer(gameType, battlefieldSize,
                playerOneName, playerTwoName) {SelectableFieldRowCount = battlefieldSize[1]};
            BattleManager battle = new BattleManager(gameRenderer, _playerOneField,
                _playerTwoField, playerOneName, playerTwoName, shipsCapacity) {GameMode = gameType};
            GameEventListener gameEventListener = new GameEventListener(battle, gameRenderer);
            battle.EventListener = gameEventListener;
            battle.Start();
        }

        private string AskPlayerName(int playersCount = 1, string currentPlayerNumber = "") {
            Console.Clear();
            Console.CursorVisible = true;
            if (playersCount == 1) {
                Console.Write(Color.YellowText + "Please enter your name: " + Color.Reset);
            }
            else {
                Console.Write(Color.YellowText + $"Please enter {currentPlayerNumber} name: " + Color.Reset);
            }
            string playerName = Console.ReadLine();
            if (playerName == null || playerName.Length <= 1) {
                playerName = "Human";
            }
            Console.CursorVisible = false;
            return playerName;
        }

        private string AskAiName() {
            string[] names = {"Deep Blue", "AlphaZero", "Pleo", "AIBO", "QRIO", "IBM Watson", "MYCIN", "20Q", "Wall-E", 
                "Digital Genius", "Skynet", "T1000", "Mart Calmo", "Yandex Alisa", "Apple Siri", "HAL 9000", "TARS", 
                "R2D2", "RoboCop", "Blue Brain Project", "AlphaGo", "iPavlov", "Donald Trump", "Matrix", "COVID-19"};
            return names[new Random().Next(0, names.Length)];
        }

    }
}