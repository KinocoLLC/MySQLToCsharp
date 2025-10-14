using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp;

public partial class Strings
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public required string S { get; set; }
    [StringLength(50)]
    public required string NS { get; set; }
    [Required]
    public required string T { get; set; }
    public required string NT { get; set; }
    [Required]
    public required string MT { get; set; }
    public required string NMT { get; set; }
    [Required]
    public required string LT { get; set; }
    public required string NLT { get; set; }
}
