
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
        [MaxLength(8)]
        public byte[] NullableData { get; set; }
        [Required]
        [MaxLength(128)]
        public byte[] VarData { get; set; }
        [MaxLength(512)]
        public byte[] NullableVarData { get; set; }
        [Required]
        public byte[] Tiny { get; set; }
        public byte[] NullableTiny { get; set; }
        [Required]
        public byte[] Blob { get; set; }
        public byte[] NullableBlob { get; set; }
        [Required]
        public byte[] Medium { get; set; }
        public byte[] NullableMedium { get; set; }
        [Required]
        public byte[] Long { get; set; }
        public byte[] NullableLong { get; set; }
    }
}
