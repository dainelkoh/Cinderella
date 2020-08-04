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
     public class EditModel : PageModel
     {
          private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EditModel(Cinderella.Models.CinderellaContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
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

          public async Task<IActionResult> OnPostAsync()
          {
               if (!ModelState.IsValid)
               {
                    return Page();
               }

               ApplicationRole appRole = await _roleManager.FindByIdAsync(ApplicationRole.Id);

               appRole.Id = ApplicationRole.Id;
               appRole.Name = ApplicationRole.Name;
               appRole.Description = ApplicationRole.Description;

               IdentityResult roleRuslt = await _roleManager.UpdateAsync(appRole);

            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Edit Role";
            auditrecord.DateTimeStamp = DateTime.Now;

            var userID = User.Identity.Name.ToString();
            auditrecord.Desc = String.Format("User Role called '{0}' was edited by {1}", ApplicationRole.Name, userID);

            auditrecord.Username = userID;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
          }
     }
}