using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeredAngel.Database.Models
{
    public class LootItemEntity
    {
        [Key]
        public int LootItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("PlayerInventoryEntity")]
        public int InventoryId { get; set; }
        public PlayerInventoryEntity Inventory { get; set; }
    }
}
