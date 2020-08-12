using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Cinderella.Pages.Payment
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
            try { 
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });

            
            
                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(shoe.Price) * 100,   //Need to change the amount to shopping cart page
                    Description = shoe.Name,
                    Currency = "sgd",
                    Customer = customer.Id,
                    ReceiptEmail = stripeEmail // Send email receipt to customer
                });
                
                
                if (charge.Status == "succeeded")
                {

                    var user = await _userManager.GetUserAsync(User);
                    Bought QueryBought = new Bought { Id = user.Id, ShoeID = shoe.ShoeID };
                    var bought = await _context.bought.FirstOrDefaultAsync(m => m.Id == QueryBought.Id && m.ShoeID == QueryBought.ShoeID);

                    if (bought == null)
                    {
                        _context.bought.Add(QueryBought);
                        await _context.SaveChangesAsync();
                    }
                    string receipturl = charge.ReceiptUrl;
                    string subject = "Cinderella Order Confirmation";
                    string To = charge.ReceiptEmail;
                    string Body = string.Format("Thanks for shopping with Cinderella \nTransaction No. :{0}\nAmount paid: ${1}\nYour order for {2} will be shipped to you shortly. Alternatively, you may view your e-receipt at {3}.",charge.Id, charge.Amount/100, shoe.Name,receipturl);
                    MailMessage mail = new MailMessage();
                    mail.To.Add(To);
                    mail.Subject = subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = false;
                    mail.From = new MailAddress("cinderella.shoesg@gmail.com");
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        UseDefaultCredentials = true,
                        EnableSsl = true,             //use SSL to secure connection
                        Credentials = new System.Net.NetworkCredential("cinderella.shoesg@gmail.com", "Cinderella123!")
                    };
                    await smtp.SendMailAsync(mail);
                    if(charge.ReceiptEmail != user.Email)
                    {
                        string nsubject = "Cinderella Order Confirmation";
                        string nTo = user.Email;
                        string nBody = string.Format("An order for {0} was made from your account\nIf this is not you, please contact us immediately", shoe.Name);
                        MailMessage nmail = new MailMessage();
                        nmail.To.Add(nTo);
                        nmail.Subject = nsubject;
                        nmail.Body = nBody;
                        nmail.IsBodyHtml = false;
                        nmail.From = new MailAddress("cinderella.shoesg@gmail.com");
                        await smtp.SendMailAsync(nmail);
                    }
                    TransactionLog log = new TransactionLog { Id = user.Id, TransactionNumber = charge.Id, Time = DateTime.Now };
                    _context.TransactionLogs.Add(log);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Success_Page");
                }
                else
                {
                    //should make this display the error, like 'insufficient funds' or smth
                    return RedirectToPage("./Fail_Transaction");

                }
                
            }
            catch
            {   
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
