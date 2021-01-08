using System;

namespace ConsoleApp {
    
    [Serializable] public class Settings {
        public bool ShipArrangement { get; set; }
        public int[] ShipCount { get; set; }
        public int[] ShipSettings { get; set; }
        public int[] BattlefieldSize { get; set; }
        public string[] ShipNames { get; set; }
    }
}