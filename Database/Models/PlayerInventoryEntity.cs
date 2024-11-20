using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeredAngel.Database.Models
{
    public class PlayerInventoryEntity
    {
        [Key]
        public int InventoryId { get; set; }

        public List<LootItemEntity> LootItems { get; set; } = new List<LootItemEntity>();
    }
}
