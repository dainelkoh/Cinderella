using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinderella.Pages.Reviews
{
    [Authorize(Roles = "Staff, Admin")]
    public class ManageAllReviewsModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageAllReviewsModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Shoe Shoe { get; set; }
        public List<Shoe> Shoes { get; set; }
        public ReviewFinal Review { get; set; }
        public List<ReviewFinal> Reviews { get; set; }
        public List<List<ReviewFinal>> ReviewsList { get; set; }
        public List<bool> IsDeleteableList { get; set; }
        public bool IsDisplayable { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            var ShoesQ = from s in _context.Shoe
                           select s;
            Shoes = await ShoesQ.ToListAsync();
            if (Shoes == null)
            {
                IsDisplayable = true;
                return RedirectToPage("../Manage");
            }

            IsDeleteableList = new List<bool>();
            ReviewsList = new List<List<ReviewFinal>>();
            for(int i = 0; i < Shoes.Count; i++)
            {
                Review = await _context.ReviewFinals.FirstOrDefaultAsync(m => m.ShoeID == Shoes[i].ShoeID);
                IsDeleteableList.Add(true);
                if (Review == null)
                {
                    ReviewFinal NullReview = new ReviewFinal
                    {
                        ReviewID = 0,
                        ShoeID = Shoes[i].ShoeID,
                        ReviewName = "No Reviews",
                        ReviewWords = "No Reviews"
                    };
                    IsDeleteableList[i] = false;
                    Reviews = new List<ReviewFinal> { NullReview };
                    ReviewsList.Add(Reviews);
                }
                else
                {
                    var ReviewsQ = from s in _context.ReviewFinals where s.ShoeID == Shoes[i].ShoeID
                                   select s;
                    ReviewsList.Add(ReviewsQ.ToList());
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int reviewId, int shoeId)
        {
            Shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == shoeId);
            if (Shoe == null)
            {
                IsDisplayable = true;
                return RedirectToPage("../Manage");
            }

            Review = await _context.ReviewFinals.FirstOrDefaultAsync(m => m.ReviewID == reviewId);
            if (Review == null)
            {
                return Page();
            }
            _context.ReviewFinals.Remove(Review);
            await _context.SaveChangesAsync();

            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Remove review";
            auditrecord.DateTimeStamp = DateTime.Now;

            var userID = User.Identity.Name.ToString();
            auditrecord.Desc = String.Format("Shoe review with Review id:{0} from Shoe id:{2} was removed by {1}", reviewId, userID, shoeId);

            auditrecord.Username = userID;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Reviews/ManageAllReviews");
        }
    }
}