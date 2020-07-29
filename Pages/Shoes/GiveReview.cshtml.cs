using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinderella.Pages.Shoes
{
    public class GiveReviewModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        public GiveReviewModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ReviewDesc ReviewDesc { get; set; }
        public Shoe Shoe { get; set; }
        public int Id { get; set; }
        public Review Review { get; set; }
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

            Review Review = new Review
            {
                ShoeID = id
            };


            ReviewDesc.ReviewID = Review.ReviewID;
            _context.reviews.Add(Review);
            await _context.SaveChangesAsync();
            _context.reviewDescs.Add(ReviewDesc);

            await _context.SaveChangesAsync();
            return RedirectToPage("./Details/", new { id = id });
        }
    }
}