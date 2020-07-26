using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinderella.Models;
using Stripe;


namespace Cinderella.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Charge(PayModelView data)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions {
                Email = data.emailaddr,
                Source = data.Token
             });

            var charge = charges.Create(new ChargeCreateOptions {
                Amount = Convert.ToInt32(data.Total),   //Need to change the amount to shopping cart page
                Description = "Test Payment",
                Currency = "sgd",
                Customer = customer.Id,
                ReceiptEmail = data.emailaddr, // Send email receipt to customer
            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return View();
                //return RedirectToPage("../Areas/Identity/Pages/Account/ConfirmationPage");
                //C:\Users\SSDStudent\source\repos\Cinderella4\Areas\Identity\Pages\Account\Success_Page.cshtml
                //C:\Users\SSDStudent\source\repos\Cinderella4\Controllers\HomeController.cs
                
            }
            else
            {
                //return RedirectToPage("../Areas/Identity/Pages/Account/Success_Page");
                //return View();
            }

            return View();
        }
        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    var result = await Charge(;
        //    r
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
//Links (Do Not Delete)
//https://www.youtube.com/watch?v=Iisp6g88IU4
//https://jsfiddle.net/ywain/awgfcpgj/
//Making receipt template
//https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/
