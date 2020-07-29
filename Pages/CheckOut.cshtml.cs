using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Cinderella.Pages
{
    [Authorize]
    public class CheckOutModel : PageModel
    {

        public async Task<IActionResult> OnPostAsync(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500,   //Need to change the amount to shopping cart page
                Description = "Test Payment",
                Currency = "sgd",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail // Send email receipt to customer
            });
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return RedirectToPage("./Success_Page");
                

            }
            else
            {
                return RedirectToPage("./Fail_Transaction");

            }

        }

        public void OnGet()
        {

        }
    }
}


//Links (Do Not Delete)
//https://www.youtube.com/watch?v=Iisp6g88IU4
//https://jsfiddle.net/ywain/awgfcpgj/
//Making receipt template
//https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/
