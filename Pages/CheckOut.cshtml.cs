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
using Stripe;

namespace Cinderella.Pages
{
    [Authorize]
    public class CheckOutModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CheckOutModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Shoe shoe { get; set; }
        public int ids { get; set; }
        public Bought bought { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ids = (int)id;

            shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == id);
            if (shoe == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string stripeEmail, string stripeToken, int? id)
        {

            shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == id);
            if (shoe == null)
            {
                return NotFound();
            }

            var customers = new CustomerService();
            var charges = new ChargeService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(shoe.Price)*100,   //Need to change the amount to shopping cart page
                Description = "Test Payment",
                Currency = "sgd",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail // Send email receipt to customer
            });
            if (charge.Status == "succeeded")
            {

                var user = await _userManager.GetUserAsync(User);
                Bought QueryBought = new Bought { Id = user.Id, ShoeID = shoe.ShoeID };
                var bought = await _context.bought.FirstOrDefaultAsync(m => m == QueryBought);

                if(bought == null)
                {
                    _context.bought.Add(QueryBought);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("./Success_Page");
            }
            else
            {
                //should make this display the error, like 'insufficient funds' or smth
                return RedirectToPage("./Fail_Transaction");

            }

        }

    }
}


//Links (Do Not Delete)
//https://www.youtube.com/watch?v=Iisp6g88IU4
//https://jsfiddle.net/ywain/awgfcpgj/
//Making receipt template
//https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/
