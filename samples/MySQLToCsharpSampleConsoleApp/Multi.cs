// ------------------------------------------------------------------------------
// <auto-generated>
// Code Generated by MySQLToCsharp
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySQLToCsharpSampleConsoleApp
{
    public partial class Multi
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        public int MasterId { get; set; }
        [Required]
        [StringLength(100)]
        public string Desc { get; set; }
    }
}
