using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Cinderella.Areas.Identity.Pages.Account;

namespace Cinderella.Pages.Shoes
{
    [Authorize(Roles = "Admin, Staff")]
    public class CreateModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly IHostingEnvironment _iweb;

        public CreateModel(Cinderella.Models.CinderellaContext context, IHostingEnvironment iweb)
        {
            _context = context;
            _iweb = iweb;
        }

        public IActionResult OnGet()
        {
            //throw new Exception("Test Error");
            return Page();
        }

        [BindProperty]
        public Shoe Shoe { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile uploadfiles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (uploadfiles != null)
            {
                string imgext = Path.GetExtension(uploadfiles.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var img = Path.Combine(_iweb.WebRootPath, "images", uploadfiles.FileName);
                    var stream = new FileStream(img, FileMode.Create);
                    await uploadfiles.CopyToAsync(stream);
                    stream.Close();

                    Shoe.Image = uploadfiles.FileName;
                    _context.Shoe.Add(Shoe);
                }
            }

            if (await _context.SaveChangesAsync()>0)
            {
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Add Shoe Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyShoeFieldID = Shoe.ShoeID;

                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;

                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Manage");
        }
    }
}