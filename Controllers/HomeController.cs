//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Cinderella.Models;
//using Stripe;
//using System.Diagnostics;

//namespace Cinderella.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        //public IActionResult Charge(string stripeEmail, string stripeToken)
//        //{
//        //    var customers = new CustomerService();
//        //    var charges = new ChargeService();

//        //    var customer = customers.Create(new CustomerCreateOptions {
//        //        Email = stripeEmail,
//        //        Source = stripeToken
//        //     });

//        //    var charge = charges.Create(new ChargeCreateOptions {
//        //        Amount = 500,   //Need to change the amount to shopping cart page
//        //        Description = "Test Payment",
//        //        Currency = "sgd",
//        //        Customer = customer.Id,
//        //        ReceiptEmail = stripeEmail // Send email receipt to customer
//        //    });

//        //    if (charge.Id == "")
//        //    {

//        //        //eturn RedirectToPage("./Index");
//        //        return RedirectToPage("./Index");

                
//        //    }
//        //    else
//        //    {
//        //        string BalanceTransactionId = charge.BalanceTransactionId;
//        //        return RedirectToPage("./About Us");
//        //        //return View("Views/Controllers/Success_Page.cshtml");
//        //        //return View();
//        //    }

//        //    //return View();
//        //}

//    //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    //    public IActionResult Error()
//    //    {
//    //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    //    }
//    }

//}
