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
        [System.ComponentModel.DataAnnotations.MaxLength(32)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string ReviewName { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(512)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string ReviewWords { get; set; }
    }
}
