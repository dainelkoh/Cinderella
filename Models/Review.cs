using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinderella.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [ForeignKey("Shoe")]
        public int ShoeID { get; set; }
    }
}
