using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; }
        public void OnGet(string name)
        {
            Name = name;
        }

        public void OnPost()
        {
            // var size = new Size();
            //
            // var shipOptions = new[]
            // {
            //     new ShipOptions("One", 1, 5),
            //     new ShipOptions("Two", 2, 4),
            //     new ShipOptions("Three", 3, 3),
            //     new ShipOptions("Four", 4, 2),
            //     new ShipOptions("Five", 5, 1),
            // };
            //
            // var factory = new GameFactory();
            
            // var game = factory.CreatePvA(factory.PlayerFactory.Create("Kumo", new RandomHitSelector()));
            
            // game.Player1.Flotilla
            //     .AddShip("One", new Coordinates(1, 2), Orientation.Horizontal)
            //     .AddShip("Two", new Coordinates(5, 6), Orientation.Vertical)
            //     .AddShip("Three", new Coordinates(3, 0), Orientation.Vertical);
        }
    }
}