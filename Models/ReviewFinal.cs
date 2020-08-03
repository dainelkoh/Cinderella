using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinderella.Models
{
    public class ReviewFinal
    {
        [Key]
        public int ReviewID { get; set; }

        [ForeignKey("Shoe")]
        public int ShoeID { get; set; }
        
        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string ReviewName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 10, ErrorMessage ="The review is limited to 10 and 256 letters.")]
        public string ReviewWords { get; set; }
    }
}
