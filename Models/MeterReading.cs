using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ensek.Models
{
    public class MeterReading
    {
        [Key]
        [Column(Order = 1)]
        public int AccountId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime MeterReadingDateTime { get; set; }
        [Required]
        public int MeterReadValue { get; set; }
    }
}
