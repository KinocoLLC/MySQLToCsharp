
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp
{
    public partial class Strings
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string S { get; set; }
        [Required]
        [StringLength(50)]
        public string NS { get; set; }
        [Required]
        public string T { get; set; }
        [Required]
        public string MT { get; set; }
        [Required]
        public string LT { get; set; }
    }
}
