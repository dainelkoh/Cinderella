using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinderella.Models
{
     public class Shoe
     {
          public int ShoeID { get; set; }
          public string Name { get; set; }
          public string Image { get; set; }
          public string Description { get; set; }
          [Column(TypeName = "decimal(18, 2)")]
          public decimal Price { get; set; }
     }
}
