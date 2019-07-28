
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp
{
    public partial class Room
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        public int RowVersion { get; set; }
    }
}
