using EngineeredAngel.Database.Models;
using EngineeredAngel.Loot;

namespace EngineeredAngel.Factories
{
    public class LootFactory
    {
        public static LootItem CreateLootItem(LootItemEntity entity)
        {
            var loot = new LootItem
            {
                Tier = entity.Tier,
                Name = entity.Name,
                Type = entity.Type,
                Rarity = entity.Rarity,
                Quantity = entity.Quantity,
                Attack = entity.Attack,
                Defense = entity.Defense,
                SpecialEffect = entity.SpecialEffect,
                AmplifiedDamage = entity.AmplifiedDamage,
            };

            return loot;
        }
    }
}