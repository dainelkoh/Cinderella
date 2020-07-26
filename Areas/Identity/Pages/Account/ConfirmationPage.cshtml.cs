using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Cinderella.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmationPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}