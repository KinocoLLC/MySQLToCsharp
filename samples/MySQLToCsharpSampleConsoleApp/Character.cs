// Code generated by SqlToCsharp

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySQLToCsharpSampleConsoleApp
{
    public partial class Character
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        public int MasterId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public int Exp { get; set; }
    }
}
