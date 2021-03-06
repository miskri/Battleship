﻿using System;
using System.Collections.Generic;
using ConsoleApp.Objects;

namespace ConsoleApp.Control {
    
    // DO NOT TOUCH IT, JUST USE IF SUDDENLY NEEDED. I haven't rewritten anything since I wrote this, sorry.
    public static class FieldManager {
        
        private static readonly (int, int)[] PosCheck = {(1, 0), (1, -1), (1, 1), (0, 1), (0, -1), (-1, 0), (-1, 1), (-1, -1)};

        private static readonly Random Rand = new Random();

        public static (string[,], Flotilla) GenerateField(Settings settings) {
            Random rand = new Random();
            int height = settings.BattlefieldSize[0], weight = settings.BattlefieldSize[1];
            string[,] field = new string[height, weight];
            Flotilla flotilla = new Flotilla();
            for (int i = 0; i < settings.ShipCount.Length; i++) {
                for (int j = 0; j < settings.ShipCount[i]; j++) {
                    while (true) {
                        int h = rand.Next(0, height);
                        int w = rand.Next(0, weight);
                        List<string> directions = GetAvailableDirections(settings.ShipArrangement, field, h, w, settings.ShipSettings[i]);
                        if (directions.Count == 0) continue;
                        Ship ship;
                        (field, ship) = PutShip(settings.ShipArrangement, field, directions[Rand.Next(0, directions.Count)], h, w, settings.ShipSettings[i]);
                        ship.Name = settings.ShipNames[i];
                        flotilla.AddShip(ship);
                        break;
                    }
                }
            }

            return (field, flotilla);
        }

        public static List<string> GetAvailableDirections(bool contact, string[,] field, int row, int col, int shipSize) {
            if (field[row, col] != null && field[row, col] != "st true" && field[row, col] != "st false") return new List<string>();
            List<string> directions = new List<string>();
            if (!contact) {
                for (int i = 0; i < shipSize; i++) {
                    if (!CheckAround(field, row + i, col)) break;
                    if (i == shipSize - 1) {
                        directions.Add("right");
                    }
                }

                for (int j = 0; j < shipSize; j++) {
                    if (!CheckAround(field, row, col + j)) break;
                    if (j == shipSize - 1) {
                        directions.Add("down");
                    }
                }
            }
            else {
                for (int a = 0; a < shipSize; a++) {
                    if (row + a >= field.GetLength(0) || field[row + a, col] != null && field[row + a, col] != "st true") break;
                    if (a == shipSize - 1) {
                        directions.Add("right");
                    }
                }

                for (int b = 0; b < shipSize; b++) {
                    if (col + b >= field.GetLength(1) || field[row, col + b] != null && field[row, col + b] != "st true") break;
                    if (b == shipSize - 1) {
                        directions.Add("down");
                    }
                }
            }

            return directions;
        }

        private static bool CheckAround(string[,] field, int row, int col) {
            if (!(row >= 0 && row < field.GetLength(0) && col >= 0 && col < field.GetLength(1))) return false;
            foreach (var (item1, item2) in PosCheck) {
                try {
                    if (field[row + item1, col + item2] == "ship") {
                        return false;
                    }
                }
                catch (IndexOutOfRangeException) {
                }
            }

            return true;
        }

        public static (string[,], Ship) PutShip(bool contact, string[,] field, string direction, int row, int col, int shipSize) {
            (int, int)[] cells = new (int, int)[shipSize];
            if (direction == "right") {
                for (int i = 0; i < shipSize; i++) {
                    field[row + i, col] = "ship";
                    cells[i] = (row + i, col);
                    if (!contact) {
                        field = PutBordersAround(field, row + i, col);
                    }
                }
            }
            else {
                for (int j = 0; j < shipSize; j++) {
                    field[row, col + j] = "ship";
                    cells[j] = (row, col + j);
                    if (!contact) {
                        field = PutBordersAround(field, row, col + j);
                    }
                }
            }
            Ship ship = new Ship(cells);
            return (field, ship);
        }

        private static string[,] PutBordersAround(string[,] field, int row, int col) {
            foreach (var (item1, item2) in PosCheck) {
                try {
                    if (field[row + item1, col + item2] == null) {
                        field[row + item1, col + item2] = "border";
                    }
                }
                catch (IndexOutOfRangeException) {
                }
            }

            return field;
        }
    }
}