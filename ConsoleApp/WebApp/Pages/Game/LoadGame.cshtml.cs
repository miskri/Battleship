using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain.Objects;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class LoadGameModel : PageModel
    {
        private readonly AppDbContext _context;
        

        public LoadGameModel(AppDbContext context)
        {
            _context = context;
        }

        public List<SaveObject> Saves = default!;

        public async Task OnGetAsync()
        {
            Saves = await _context.Saves.ToListAsync();
        }
    }
}
