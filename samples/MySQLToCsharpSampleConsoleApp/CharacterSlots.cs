
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MySQLToCsharpSampleConsoleApp
{
    public partial class CharacterSlots
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        public int SlotIndex { get; set; }
        public int CharacterId { get; set; }
        public int WeaponId { get; set; }
        public int WeaponId2 { get; set; }
        public int Exp { get; set; }
    }
}
