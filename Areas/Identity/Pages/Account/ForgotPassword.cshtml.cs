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
using System.Security.Cryptography;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Cinderella.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, ILogger<ForgotPasswordModel> logger, IConfiguration configuration, IEmailSender emailSender)
        {
            this.configuration = configuration;
            this.logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            var client = HttpClientFactory.Create();
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"secret", configuration.GetSection("reCAPTCHA").GetValue<string>("SecretKey")},
                    {"response", recaptchaResponse},
                    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
                };

                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    this.ModelState.AddModelError(string.Empty, "Please verify that you are a human.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Something went wrong with the API. Let the request through.
                this.logger.LogError(ex, "Unexpected error calling reCAPTCHA api.");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                //|| !(await _userManager.IsEmailConfirmedAsync(user)
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

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

                var test = await _userManager.DeleteAsync(user);

                if (test.Succeeded)
                {
                    var new_user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                    var result = await _userManager.CreateAsync(new_user, s);

                    if (result.Succeeded)
                    {
                        string subject = "Cinderella Shoes Password Reset";
                        string To = Input.Email;
                        string Body = "Your password is:   " + s + "   Please login to your account using the password given to you and change the password upon login to your account. If you have not request for password to be reset, please contact us immediately at Cinderellashoesg@gmail.com.";
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
                        return RedirectToPage("./ForgotPasswordConfirmation");


                        //// For more information on how to enable account confirmation and password reset please 
                        //// visit https://go.microsoft.com/fwlink/?LinkID=532713
                        //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        //var callbackUrl = Url.Page(
                        //    "/Account/ResetPassword",
                        //    pageHandler: null,
                        //    values: new { code },
                        //    protocol: Request.Scheme);

                        //await _emailSender.SendEmailAsync(
                        //    Input.Email,
                        //    "Reset Password",
                        //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    else
                    {
                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }
                }
                else
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                
            }
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
