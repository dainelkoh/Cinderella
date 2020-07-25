using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Shoes
{
    [Authorize(Roles = "Admin, Staff")]
    public class CreateModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;

        public CreateModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Shoe Shoe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Shoe.Add(Shoe);

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

            return RedirectToPage("./Index");
        }
    }
}