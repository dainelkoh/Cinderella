﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Cinderella.Pages.Payment
{
    [Authorize]
    public class Fail_TransactionModel : PageModel
    {
        //public String e { get; set; }    
        //public void OnGet(Exception E)
        //{
        //    e = E.Message;
        //}
    }
}