using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cinderella.Models
{
     public class Shoe
     {
        internal int Shoesid;

        public int ShoeID { get; set; }

          [Required]
          public string Name { get; set; }

          public string Image { get; set; }
          
          public string Description { get; set; }

          [Column(TypeName = "decimal(6, 2)")]
          [Required]
          public decimal Price { get; set; }
    }
}
