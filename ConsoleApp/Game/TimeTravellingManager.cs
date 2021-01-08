using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp {
    
    public class TimeTravellingManager {
        
        private List<GameProperties> _history;
        private int _stepsBackCount;

        public void WritePreviousStep(GameProperties props) {
            if (_history == null) LoadRoundHistory(props.Id);
            
            if (_history.Count == _stepsBackCount) _history.RemoveAt(_stepsBackCount - 1);
            props.LoadPlayer1FieldToArray();
            props.LoadPlayer2FieldToArray();
            GameProperties propsToWrite = props.DeepClone();
            propsToWrite.Round--;
            _history.Insert(0, propsToWrite);
            }

        public GameProperties GetPreviousStep(GameProperties props) {
            if (_history == null) LoadRoundHistory(props.Id);
            _stepsBackCount--;
            GameProperties result = _history[0].DeepClone();
            _history.RemoveAt(0);
            if (_stepsBackCount == 0) result.MenuOptions.Remove("Step back");
            return result;
        }

        public void SaveRoundHistory(string id) {
            string path = $"{Data.Path}RoundHistory\\{id}.json";
            if (!File.Exists(path)) {
                FileStream stream = File.Create(path);
                stream.Close();
            }

            RoundHistory history = new RoundHistory(_stepsBackCount, _history.ToArray());
            
            StreamWriter file = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(history, Data.JsonOptions);
            
            file.Write(jsonString);
            file.Close();
        }

        public int GetStepsBackCount(string id) {
            if (_history == null) LoadRoundHistory(id);
            return _stepsBackCount;
        }

        private void LoadRoundHistory(string id) {
            if (!File.Exists($"{Data.Path}RoundHistory\\{id}.json")) {
                _history = new List<GameProperties>();
                _stepsBackCount = 5;
                return;
            }

            string pathToHistory = $"{Data.Path}RoundHistory\\{id}.json";
            StreamReader file = new StreamReader(pathToHistory);

            string jsonString = file.ReadToEnd();
            RoundHistory history = JsonSerializer.Deserialize<RoundHistory>(jsonString);
            file.Close();

            _history = history.History.ToList();
            _stepsBackCount = history.StepsBackCount;
        }
    }

    [Serializable] class RoundHistory {
        public int StepsBackCount { get; set; }
        public GameProperties[] History { get; set; }
        
        public RoundHistory() {}

        public RoundHistory(int stepsBackCount, GameProperties[] history) {
            StepsBackCount = stepsBackCount;
            History = history;
        }
    }
}