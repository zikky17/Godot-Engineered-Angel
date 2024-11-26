using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeredAngel.Database.Models
{
    public class PlayerInventoryEntity
    {
        [Key]
        public int InventoryId { get; set; }

        public int MaxSlots { get; set; }
        public ICollection<LootItemEntity> LootItems { get; set; } = new List<LootItemEntity>();
    }
}
