using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Roles
{
     [Authorize(Roles = "Admin")]
     public class DeleteModel : PageModel
     {
          private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
          {
               _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

          [BindProperty]
          public ApplicationRole ApplicationRole { get; set; }

          public async Task<IActionResult> OnGetAsync(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               ApplicationRole = await _roleManager.FindByIdAsync(id);

               if (ApplicationRole == null)
               {
                    return NotFound();
               }
               return Page();
          }

          public async Task<IActionResult> OnPostAsync(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               ApplicationRole = await _roleManager.FindByIdAsync(id);
               IdentityResult roleRuslt = await _roleManager.DeleteAsync(ApplicationRole);
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Delete Role";
            auditrecord.DateTimeStamp = DateTime.Now;

            var userID = User.Identity.Name.ToString();
            auditrecord.Desc = String.Format("User Role called '{0}' was deleted by {1}", ApplicationRole.Name, userID);
            auditrecord.Username = userID;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
          }
     }
}

