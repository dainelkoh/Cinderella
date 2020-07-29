using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Cinderella.Pages.Shoes
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DetailsModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Shoe Shoe { get; set; }
        public Review Review { get; set; }
        public IList<ReviewDesc> ReviewDesc { get; set; }
        public bool Reviewable { get; set; }

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

            var user = await _userManager.GetUserAsync(User);
            Bought QueryBought = new Bought { Id = user.Id, ShoeID = Shoe.ShoeID };
            var bought = await _context.bought.FirstOrDefaultAsync(m => m == QueryBought);

            if (bought == null)
            {
                Reviewable = false;
            }
            else
            {
                Reviewable = true;
            }


            Review = await _context.reviews.FirstOrDefaultAsync(m => m.ShoeID == id);
            if(Review == null)
            {
                ReviewDesc NullReview = new ReviewDesc
                {
                    ReviewDescID = 0,
                    ReviewID = 0,
                    ReviewName = "No Reviews",
                    ReviewWords = "No Reviews"
                };
                ReviewDesc = new List<ReviewDesc> { NullReview };
            }
            else
            {
                var reviewDesc = from s in _context.reviewDescs
                                 select s;
                ReviewDesc = await reviewDesc.ToListAsync();
            }
            return Page();
        }
    }
}
