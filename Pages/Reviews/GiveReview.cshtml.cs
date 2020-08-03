using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Reviews
{
    [Authorize]
    public class GiveReviewModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        public GiveReviewModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
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