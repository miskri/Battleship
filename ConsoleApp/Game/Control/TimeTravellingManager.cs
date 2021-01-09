using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleApp.Data;
using ConsoleApp.Utils;

namespace ConsoleApp.Control {
    
    public class TimeTravellingManager {
        
        private List<GameProperties> _history;
        private int _stepsBackCount;
        private const string Path = "C:\\Users\\Mihhail\\RiderProjects\\icd0008-2020f\\ConsoleApp\\Game\\RoundHistory\\";


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
            if (!File.Exists($"{Path}{id}.json")) {
                FileStream stream = File.Create($"{Path}{id}.json");
                stream.Close();
            }

            RoundHistory history = new RoundHistory(_stepsBackCount, _history.ToArray());
            
            StreamWriter file = new StreamWriter($"{Path}{id}.json");
            string jsonString = JsonSerializer.Serialize(history, DataUtils.JsonOptions);
            
            file.Write(jsonString);
            file.Close();
        }

        public int GetStepsBackCount(string id) {
            if (_history == null) LoadRoundHistory(id);
            return _stepsBackCount;
        }

        private void LoadRoundHistory(string id) {
            if (!File.Exists($"{Path}{id}.json")) {
                _history = new List<GameProperties>();
                _stepsBackCount = 5;
                return;
            }

            string pathToHistory = $"{Path}{id}.json";
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