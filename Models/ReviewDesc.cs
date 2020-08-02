using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinderella.Models
{
    public class ReviewDesc
    {
        public int ReviewDescID { get; set; }

        [ForeignKey("Review")]
        public int ReviewID { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(32)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string ReviewName { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(512)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string ReviewWords { get; set; }
    }
}
