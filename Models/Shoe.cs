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
          public int ShoeID { get; set; }
          [StringLength(30, MinimumLength = 3)]
          [RegularExpression(@"^[A-Z]+[a-zA-Z0-9]*$")]
          [Required]
          public string Name { get; set; }

          [Url]
          public string Image { get; set; }

          [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
          [StringLength(60)]
          public string Description { get; set; }

          [Range(1, 1000)]
          [DataType(DataType.Currency)]
          [Column(TypeName = "decimal(18, 2)")]
          [Required]
          public decimal Price { get; set; }
    }
}
