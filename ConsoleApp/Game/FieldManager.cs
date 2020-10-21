using System;
using System.Collections.Generic;

namespace ConsoleApp {
    public class FieldManager {
        private readonly (int, int)[] _posCheck = {(1, 0), (1, -1), (1, 1), (0, 1), (0, -1), (-1, 0), (-1, 1), (-1, -1)};

        private Random _rand = new Random();

        public string[,] GenerateField(bool contact, int[] shipCount, int[] shipSettings, int[] battlefieldSize) {
            Random rand = new Random();
            int height = battlefieldSize[1], weight = battlefieldSize[0];
            string[,] field = new string[height, weight];
            for (int i = 0; i < shipCount.Length; i++) {
                for (int j = 0; j < shipCount[i]; j++) {
                    bool putted = false;
                    while (!putted) {
                        int h = rand.Next(0, height);
                        int w = rand.Next(0, weight);
                        List<string> directions = CheckPosition(contact, field, h, w, shipSettings[i]);
                        if (directions.Count == 0) continue;
                        field = PutShip(contact, field, directions[_rand.Next(0, directions.Count)], h, w, shipSettings[i]);
                        putted = true;
                    }
                }
            }

            return field;
        }

        public List<string> CheckPosition(bool contact, string[,] field, int x, int y, int shipSize) {
            if (field[x, y] != null) return new List<string>();
            List<string> directions = new List<string>();
            if (!contact) {
                for (int i = 0; i < shipSize; i++) {
                    if (!CheckAround(field, x + i, y)) break;
                    if (i == shipSize - 1) {
                        directions.Add("right");
                    }
                }

                for (int j = 0; j < shipSize; j++) {
                    if (!CheckAround(field, x, y + j)) break;
                    if (j == shipSize - 1) {
                        directions.Add("down");
                    }
                }
            }
            else {
                for (int a = 0; a < shipSize; a++) {
                    if (x + a >= field.GetLength(0) || field[x + a, y] != null) break;
                    if (a == shipSize - 1) {
                        directions.Add("right");
                    }
                }

                for (int b = 0; b < shipSize; b++) {
                    if (y + b >= field.GetLength(1) || field[x, y + b] != null) break;
                    if (b == shipSize - 1) {
                        directions.Add("down");
                    }
                }
            }

            return directions;
        }

        private bool CheckAround(string[,] field, int x, int y) {
            if (!(x >= 0 && x < field.GetLength(0) && y >= 0 && y < field.GetLength(1))) return false;
            foreach (var (item1, item2) in _posCheck) {
                try {
                    if (field[x + item1, y + item2] != null) {
                        return false;
                    }
                }
                catch (IndexOutOfRangeException e) {
                }
            }

            return true;
        }

        public string[,] PutShip(bool contact, string[,] field, string direction, int x, int y, int shipSize ) {
            if (direction == "right") {
                for (int i = 0; i < shipSize; i++) {
                    field[x + i, y] = "ship";
                    if (!contact) {
                        field = PutBordersAround(field, x + i, y);
                    }
                }
            }
            else {
                for (int j = 0; j < shipSize; j++) {
                    field[x, y + j] = "ship";
                    if (!contact) {
                        field = PutBordersAround(field, x, y + j);
                    }
                }
            }

            return field;
        }

        private string[,] PutBordersAround(string[,] field, int x, int y) {
            foreach (var (item1, item2) in _posCheck) {
                try {
                    if (field[x + item1, y + item2] == null) {
                        field[x + item1, y + item2] = "border";
                    }
                }
                catch (IndexOutOfRangeException e) {
                }
            }

            return field;
        }
    }
}