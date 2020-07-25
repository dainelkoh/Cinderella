using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Pages.Audit
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;

        public IndexModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }

        public IList<AuditRecord> AuditRecord { get;set; }

        public async Task OnGetAsync()
        {
            AuditRecord = await _context.AuditRecords.ToListAsync();
        }
    }
}
