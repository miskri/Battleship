using System;
using ConsoleApp.Control;
using ConsoleApp.Data;
using ConsoleApp.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
        
        public RedirectToPageResult OnPost()
        {
            GameManager manager = new GameManager();
            GameProperties props = new GameProperties() {Player1Field = new string[10, 10], 
                Player2Field = new string[10, 10], 
                Player1Flotilla = new Flotilla(), 
                Player2Flotilla = new Flotilla()};
            manager.LoadFastGameForWeb(props);
            string saveName = $"{props.GameMode} - {DateTime.Now}";
            var thisGameId = SaveManager.SaveGame(new Save(saveName, props));
                
            return RedirectToPage("./Game/FastGame", new {gameId = thisGameId});
        }
    }
}