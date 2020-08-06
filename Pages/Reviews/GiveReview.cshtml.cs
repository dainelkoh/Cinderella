using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Cinderella.Pages.Reviews
{
    [Authorize]
    public class GiveReviewModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GiveReviewModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public ReviewFinal Review { get; set; }
        public Shoe Shoe { get; set; }
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id = (int)id;

            Shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == id);
            if (Shoe == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Bought QueryBought = new Bought { Id = user.Id, ShoeID = Shoe.ShoeID };
                Bought bought = await _context.bought.FirstOrDefaultAsync(m => m.Id == QueryBought.Id && m.ShoeID == QueryBought.ShoeID);
                if (bought == null)
                {
                    return RedirectToPage("../Shoes/Details/", new { id = id });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.ShoeID = id;
            _context.ReviewFinals.Add(Review);
            await _context.SaveChangesAsync();
            return RedirectToPage("../Shoes/Details/", new { id = id });
        }
    }
}