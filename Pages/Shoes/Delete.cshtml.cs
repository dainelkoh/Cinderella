using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Shoes
{
    [Authorize(Roles = "Admin, Staff")]
    public class DeleteModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;

        public DeleteModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Shoe Shoe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shoe = await _context.Shoe.FirstOrDefaultAsync(m => m.ShoeID == id);

            if (Shoe == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Shoe = await _context.Shoe.FindAsync(id);

            if (Shoe != null)
            {
                _context.Shoe.Remove(Shoe);

                if (await _context.SaveChangesAsync() > 0)
                {
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "Delete Shoe Record";
                    auditrecord.DateTimeStamp = DateTime.Now;
                    auditrecord.KeyShoeFieldID = Shoe.ShoeID;
                    var userID = User.Identity.Name.ToString();
                    auditrecord.Username = userID;
                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                 }
            }

            return RedirectToPage("./Index");
        }
    }
}
