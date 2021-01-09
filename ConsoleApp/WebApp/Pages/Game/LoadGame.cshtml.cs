using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp.Control;
using ConsoleApp.Objects;
using DAL;
using Domain.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class LoadGameModel : PageModel
    {
        private readonly AppDbContext _ctx;
        
        [BindProperty]
        public string SaveName { get; set; } = string.Empty;
        

        public LoadGameModel(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<SaveObject> Saves = default!;

        public async Task OnGetAsync() {
            Saves = await _ctx.Saves.ToListAsync();
        }
        
        public RedirectToPageResult OnPost(string saveName) {
            Save save = SaveManager.GetSave(saveName);
            SaveManager.DeleteSavedGame(save);
            return RedirectToPage("./LoadGame");
        }
    }
}
