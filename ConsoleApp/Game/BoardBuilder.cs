using System.Collections.Generic;

namespace ConsoleApp {
    
    public class BoardBuilder {

        private readonly GameManager _manager;
        private readonly BoardRenderer _renderer;
        public bool Contact;
        public string[,] Field;
        public int LastRow = 0, LastCol = 0, LastDirection = 0;

        public BuilderEventListener EventListener;
        
        public BoardBuilder(GameManager manager, BoardRenderer renderer, IReadOnlyList<int> battlefieldSize) {
            _manager = manager;
            _renderer = renderer;
            Field = new string[battlefieldSize[1], battlefieldSize[0]];
        }

        public void Start(int[] shipCount, int[] shipSettings) {
            for (int i = 0; i < shipCount.Length; i++) {
                for (int j = 0; j < shipCount[i]; j++) {
                    _renderer.RenderBoardBuilder(EventListener, Field, LastRow, LastCol, shipSettings[i], 
                        LastDirection, Contact);
                }
            }
            _manager.SetPlayerField(Field);
        }
        
    }
}