using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Shoes
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;

        public DetailsModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }

        public Shoe Shoe { get; set; }
        public Review Review { get; set; }
        public IList<ReviewDesc> ReviewDesc { get; set; }

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
