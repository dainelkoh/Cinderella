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
        
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(32, MinimumLength = 1)]
        public string ReviewName { get; set; }

        [Required(ErrorMessage = "This is a required field")]
        [StringLength(256, MinimumLength = 3, ErrorMessage ="The review is limited to 3 and 256 letters.")]
        public string ReviewWords { get; set; }
    }
}
