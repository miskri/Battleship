using System.Collections.Generic;

namespace ConsoleApp {
    
    // Set of necessary data for transfer between BoardBuilder, BuilderRenderer, BuilderEventListener
    public class BuilderProperties {
        
            public string Mode { get; set; }
            public string PlayerName { get; set; }
            public string CurrentShipName { get; set; }
            public Flotilla PlayerFlotilla { get; set; }
            public string[,] PlayerField { get; set; }
            public string[] ShipNames { get; set; }
            public List<string> MenuOptions { get; set; }
            public bool ShipArrangement { get; set; }
            public int SelectableRowCount { get; set; }
            public int CursorLength { get; set; }
            public int Direction { get; set; }
            public BoardBuilder Builder;
            public BuilderRenderer Renderer;
            public BuilderEventListener EventListener;
    }
}