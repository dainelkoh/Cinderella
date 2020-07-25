using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cinderella.Models
{
     public class AuditRecord
     {
          [Key]
          public int Audit_ID { get; set; }

          [Display(Name = "Audit Action")]
          public string AuditActionType { get; set; }

          [Display(Name = "Performed By")]
          public string Username { get; set; }

          [Display(Name = "Date/Time Stamp")]
          [DataType(DataType.DateTime)]
          public DateTime DateTimeStamp { get; set; }

          [Display(Name = "Shoe Record ID ")]
          public int KeyShoeFieldID { get; set; }
     }

}

