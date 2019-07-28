
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp
{
    public partial class Weapon
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int Exp { get; set; }
    }
}
