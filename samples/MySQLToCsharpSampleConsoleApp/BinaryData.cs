
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp
{
    public partial class BinaryData
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public byte[] Data { get; set; }
        [Required]
        [MaxLength(8)]
        public byte[] NullableData { get; set; }
        [Required]
        [MaxLength(128)]
        public byte[] VarData { get; set; }
        [Required]
        [MaxLength(512)]
        public byte[] NullableVarData { get; set; }
    }
}
