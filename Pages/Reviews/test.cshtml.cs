using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinderella.Pages.Shoes
{
    public class testModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public testModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Shoe Shoe { get; set; }
        public ReviewFinal Review { get; set; }
        public IList<ReviewFinal> Reviews { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == id);
            if (Shoe == null)
            {
                return NotFound();
            }

            Review = await _context.ReviewFinals.FirstOrDefaultAsync(m => m.ShoeID == id);
            if (Review == null)
            {
                ReviewFinal NullReview = new ReviewFinal
                {
                    ReviewID = 0,
                    ShoeID = (int)id,
                    ReviewName = "No Reviews",
                    ReviewWords = "No Reviews"
                };
                Reviews = new List<ReviewFinal> { NullReview };
            }
            else
            {
                var ReviewsQ = from s in _context.ReviewFinals
                               select s;
                Reviews = await ReviewsQ.ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> RemoveReview(int reviewId, int shoeId)
        {
            Shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == shoeId);
            if (Shoe == null)
            {
                return Page();
            }

            Review = await _context.ReviewFinals.FirstOrDefaultAsync(m => m.ShoeID == shoeId);
            if (Review == null)
            {
                return Page();
            }
            _context.ReviewFinals.Remove(Review);
            return Page();
        }
    }
}