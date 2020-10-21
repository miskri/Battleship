namespace ConsoleApp {
    public class GameManager {
        private string[,] _playerOneField;
        private string[,] _playerTwoField;

        public void Start(string gameType, bool contact, int[] shipCount, int[] shipSettings, int[] battlefieldSize) {
            FieldManager fieldManager = new FieldManager();
            if (gameType == "Fast Game") {
                _playerOneField = fieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
                _playerTwoField = fieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
                BoardRenderer boardRenderer = new BoardRenderer(gameType, battlefieldSize,
                    "Human", "AI");
                BattleManager battle = new BattleManager(boardRenderer, _playerOneField,
                    _playerTwoField, "Human", "AI");
                BattleEventListener eventListener = new BattleEventListener(battle, boardRenderer);
                boardRenderer.EventListener = eventListener;
                battle.Start();
            }
            else if (gameType == "Player vs Player") {
                
            }
            else if (gameType == "Player vs AI") {
                
            }
            else {
                _playerOneField = fieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
                _playerTwoField = fieldManager.GenerateField(contact, shipCount, shipSettings, battlefieldSize);
                BoardRenderer boardRenderer = new BoardRenderer(gameType, battlefieldSize,
                    "Beavis", "Butthead");
                BattleManager battle = new BattleManager(boardRenderer, _playerOneField,
                    _playerTwoField, "Beavis", "Butthead");
                battle.Start();
            }
        }
    }
}