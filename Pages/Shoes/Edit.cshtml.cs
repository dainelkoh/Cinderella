using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;

namespace Cinderella.Pages.Shoes
{
    [Authorize(Roles = "Admin, Staff")]
    public class EditModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;
        private readonly IHostingEnvironment _iweb;

        public EditModel(Cinderella.Models.CinderellaContext context, IHostingEnvironment iweb)
        {
            _context = context;
            _iweb = iweb;
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
                }
            }            

            _context.Attach(Shoe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoeExists(Shoe.ShoeID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Manage");
        }

        private bool ShoeExists(int id)
        {
            return _context.Shoe.Any(e => e.ShoeID == id);
        }
    }
}
