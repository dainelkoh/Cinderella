using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinderella.Models
{
    public class TransactionLog
    {
        [Key]
        public string TransactionNumber { get; set; }
        [ForeignKey("AspNetUsers")]
        public string Id { get; set; }
        public DateTime Time { get; set; }
    }
}
