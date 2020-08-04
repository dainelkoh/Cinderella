using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinderella.Pages.Shoes
{
    public class DetailsModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DetailsModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public bool isAuth { get; set; }
        public Shoe Shoe { get; set; }
        public ReviewFinal Review { get; set; }
        public IList<ReviewFinal> Reviews { get; set; }
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
            if (user != null)
            {
                 Bought QueryBought = new Bought { Id = user.Id, ShoeID = Shoe.ShoeID };
                 Bought bought = await _context.bought.FirstOrDefaultAsync(m => m.Id == QueryBought.Id);
                 if (bought == null)
                 {
                     Reviewable = false;
                 }
                 else if (bought.ShoeID == QueryBought.ShoeID)
                 {
                     Reviewable = true;
                 }
                 else
                 {
                     Reviewable = false;
                 }
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
                var reviewQuery = from s in _context.ReviewFinals where s.ShoeID == Shoe.ShoeID
                                  select s;
                Reviews = await reviewQuery.ToListAsync();
            }

            //if (User.IsInRole("Staff"))
            //{
            //    isAuth = true;
            //}
            return Page();
        }
    }
}
