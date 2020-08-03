using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Cinderella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Cinderella.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration configuration;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration,
            Cinderella.Models.CinderellaContext context
            )
        {
            this.configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public ApplicationUser User1 {get; set;}
        public bool IsAlrUser { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            //[Required]
            //[Display(Name = "recaptcha")]
            //public bool recaptcha { get; set; }

        }

        public void OnGet(bool isalruser = false, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IsAlrUser = isalruser;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            var client = HttpClientFactory.Create();
            try
            {
                var parameters = new Dictionary<string, string>
            {
                {"secret", this.configuration["reCAPTCHA:SecretKey"]},
                {"response", recaptchaResponse},
                {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
            };

                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    this.ModelState.AddModelError(string.Empty, "Please verify that you are a human");
                }
            }
            catch (HttpRequestException ex)
            {
                // Something went wrong with the API. Let the request through.
                _logger.LogError(ex, "Unexpected error calling reCAPTCHA api.");
            }


            //this is  the boundary



            var User1 = await _context.Users.FirstOrDefaultAsync(m => m.Email == Input.Email);

            if (User1 != null)
            {
                IsAlrUser = true;
                return RedirectToPage("../Account/Register", new { isalruser = IsAlrUser });
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                

                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*_-+";
                int length = 20;
                string s = "";
                using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
                {
                    while (s.Length != length)
                    {
                        byte[] oneByte = new byte[1];
                        provider.GetBytes(oneByte);
                        char character = (char)oneByte[0];
                        if (valid.Contains(character))
                        {
                            s += character;
                        }
                    }
                }

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FullName = Input.FullName };
                var result = await _userManager.CreateAsync(user, s);
                if (result.Succeeded)
                {
                    string subject = "Cinderella Shoes Confirmation";
                    string To = Input.Email;
                    string Body = "Your password is:   " + s + "   Please login to your account using the password given to you and change the password upon login to your account";
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
                    return RedirectToPage("./ConfirmationPage");

                    //_logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                }
                else
                {
                    return Page();
                }
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}
