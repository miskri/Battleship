using System;

namespace ConsoleApp {
    
    [Serializable] public class Save {

        public GameProperties Properties { get; set; }
        public string SaveName { get; set; }

        public Save() {}
        public Save(string name, GameProperties props) {
            SaveName = name;
            Properties = props;
        }
    }
}