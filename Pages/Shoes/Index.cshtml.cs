﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinderella.Models;

namespace Cinderella.Pages.Shoes
{
    public class IndexModel : PageModel
    {
        private readonly Cinderella.Models.CinderellaContext _context;

        public IndexModel(Cinderella.Models.CinderellaContext context)
        {
            _context = context;
        }

        public IList<Shoe> Shoe { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var shoes = from s in _context.Shoe
                        select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                 shoes = shoes.Where(ss => ss.Name.Contains(SearchString));
            }

            Shoe = await shoes.ToListAsync();            
        }
    }
}
