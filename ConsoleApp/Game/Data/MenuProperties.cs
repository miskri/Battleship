using ConsoleApp.Control;
using ConsoleApp.EventListeners;
using ConsoleApp.Graphics;
using ConsoleApp.Objects;

namespace ConsoleApp.Data {
    
    public class MenuProperties {
        
        public MenuLevel Level { get; set; }
        public int SelectedRow { get; set; }
        public string GameMode { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public SettingsEventListener SettingsEventListener { get; set; }
        public MenuManager Manager { get; set; }
        public MenuRenderer Renderer { get; set; }
        public MenuEventListener EventListener { get; set; }
    }
}