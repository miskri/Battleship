﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp {
    public class BattleManager {
        
        public BoardRenderer Renderer { get; private set; }

        private readonly Random _rand = new Random();
        private string[,] _playerOneField, _playerTwoField;
        private string _nextPlayer;
        private readonly string _playerOneName, _playerTwoName;
        private readonly string _battleMessageMissHit = $"{Color.WhiteText}miss{Color.Reset}";
        private readonly string _battleMessageDamageHit = $"{Color.RedText}ship damaged{Color.Reset}";
        private readonly string _battleMessageShipDestroyed = $"{Color.RedText}ship destroyed{Color.Reset}";
        private int _round = 0;
        public string GameMode;
        private readonly Dictionary<string, int> _playersShipCapacity = new Dictionary<string, int>(2);
        public BattleEventListener EventListener;

        public BattleManager(BoardRenderer renderer, string[,] playerOneField, string[,] playerTwoField, 
            string playerOneName, string playerTwoName, int shipsCapacity) {
            Renderer = renderer;
            _playerOneField = playerOneField;
            _playerTwoField = playerTwoField;
            _playerOneName = playerOneName;
            _playerTwoName = playerTwoName;
            _playersShipCapacity.Add(_playerOneName, shipsCapacity);
            _playersShipCapacity.Add(_playerTwoName, shipsCapacity);
        }

        public void Start() {
            _nextPlayer = _playerOneName;
            if (GameMode != "AI vs AI") {
                Renderer.RenderBoard(_playerOneField, _playerTwoField, 0, 0, 
                    _playerOneName, EventListener);   
            }
            else {
                RunAiBattle();
            }
        }

        public bool CheckMove(string[,] fieldToCheck, int row, int col) {
            return fieldToCheck[row, col] != "collision" && fieldToCheck[row, col] != "miss";
        }

        public void MakeMove(string[,] attackedField, string[,] field, int row, int col) {
            _round++;
            switch (GameMode) {
                case "Player vs Player":
                    _nextPlayer = _nextPlayer == _playerOneName ? _playerTwoName : _playerOneName;
                    string attackingSide = _nextPlayer == _playerOneName ? _playerTwoName : _playerOneName;
                    RegisterHit(attackedField, attackingSide, _nextPlayer, row, col);
                    Thread.Sleep(2000);
                    Renderer.RenderBoard(attackedField, field, row, col, _nextPlayer, EventListener);
                    break;
                
                default: // game mode is "Player vs AI"
                    RegisterHit(attackedField, _playerOneName, _playerTwoName, row, col);
                    _round++;
                    field = AiMove(field, _playerTwoName, _playerOneName);
                    Renderer.RenderBoard(field, attackedField, row, col, _playerOneName, EventListener);
                    break;
            }
        }

        private void RegisterHit(string[,] attackedField, string attackingSide, string defendingSide, int row, int col) {
            if (attackedField[row, col] == "ship") {
                attackedField[row, col] = "collision";
                AddToBattleHistory(attackingSide, row, col, _battleMessageDamageHit);
                CheckPlayerFields(attackingSide, defendingSide);
            }
            else {
                attackedField[row, col] = "miss";
                AddToBattleHistory(attackingSide, row, col, _battleMessageMissHit);
            }
        }

        private void CheckPlayerFields(string attackingSide, string defendingSide) {
            _playersShipCapacity[defendingSide] -= 1;
            if (_playersShipCapacity[defendingSide] != 0) return;
            
            Renderer.Winner = attackingSide;
            if (Renderer.MenuOptions.Count == 3) Renderer.MenuOptions.RemoveAt(0);
            Renderer.BattleHistory.Add(attackingSide == _playerOneName
                ? $"{_playerOneName} destroys the last enemy ship!"
                : $"{_playerTwoName} destroys the last enemy ship!");
            Renderer.GameOverScreen(EventListener, _playerOneField, _playerTwoField, 0, 0);
        }

        private string[,] AiMove(string[,] field, string attackingSide, string defendingSide) {
            bool moveIsDone = false;
            while (!moveIsDone) {
                int row = _rand.Next(0, field.GetLength(0));
                int col = _rand.Next(0, field.GetLength(1));
                switch (field[row, col]) {
                    case "ship":
                        field[row, col] = "collision";
                        AddToBattleHistory(attackingSide, row, col, _battleMessageDamageHit);
                        CheckPlayerFields(attackingSide, defendingSide);
                        moveIsDone = true;
                        break;
                    case null: case "border":
                        field[row, col] = "miss";
                        AddToBattleHistory(attackingSide, row, col, _battleMessageMissHit);
                        moveIsDone = true;
                        break;
                }
            }
            return field;
        }
        
        private void RunAiBattle() {
            int roundsCount = Renderer.FieldSize[0] * Renderer.FieldSize[1] * 2;
            for (int rounds = 1; rounds <= roundsCount; rounds++) {
                if (rounds % 2 != 0) {
                    _playerTwoField = AiMove(_playerTwoField, _playerOneName, _playerOneName);
                }
                else {
                    _playerOneField = AiMove(_playerOneField, _playerTwoName, _playerTwoName);
                }

                _round++;
                Renderer.RenderBoard(_playerOneField, _playerTwoField, 0, 0, "");
                Thread.Sleep(1000);
            }
        }

        private void AddToBattleHistory(string playerName, int row, int col, string result) {
            Renderer.BattleHistory.Add(
                $"{Color.YellowText}Round {_round} - {playerName} hit {Renderer.Alphabet[col]}{row + 1}. Result - {result}{Color.YellowText}!");
        }
        
        public void SaveProgress() {
            MenuLevelDataContainer.Saves.Add($"{GameMode} - {DateTime.Now}");
            Renderer.BattleHistory.Add("Game was saved successfully!");
        }
    }
}