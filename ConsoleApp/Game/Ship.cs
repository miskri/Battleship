namespace ConsoleApp {
    
    public class Ship {

        public bool Destroyed = false;
        public readonly int Size;
        public int Health { get; private set; }
        private int[,] _shipCells;

        public Ship(int size) {
            Size = size;
            Health = Size;
            _shipCells = new int[1, Size];
            for (int i = 0; i < Size; i++) {
                _shipCells[1, i] = 1;
            }
        }

        public int SetHit(int row, int col) {
            Health -= 1;
            if (Health == 0) Destroyed = true;
            _shipCells[row, col] = 0;
            return Health;
        }
    }
}