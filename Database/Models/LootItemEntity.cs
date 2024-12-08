using EngineeredAngel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EngineeredAngel.Database.Models
{
    public class LootItemEntity
    {
        [Key]
        public int LootItemId { get; set; }
        [Required]
        public int Tier { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [AllowNull]
        public string Rarity { get; set; }
        [AllowNull]
        public int Quantity { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        [AllowNull]
        public string SpecialEffect { get; set; }
        [AllowNull]
        public int AmplifiedDamage { get; set; }
        [ForeignKey("PlayerInventoryEntity")]
        public int InventoryId { get; set; }
        public PlayerInventoryEntity Inventory { get; set; }
    }
}
