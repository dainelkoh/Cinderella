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
        public string ReviewName { get; set; }
        public string ReviewWords { get; set; }
    }
}
