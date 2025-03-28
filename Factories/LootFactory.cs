using EngineeredAngel.Database.Models;
using EngineeredAngel.Loot;
using Godot;

namespace EngineeredAngel.Factories
{
    public class LootFactory
    {
        public static LootItem CreateLootItem(LootItemEntity entity)
        {
            if(entity == null)
            {
                GD.Print("No weapon equipped");
                return null;
            }

            var loot = new LootItem
            {
                Id = entity.LootItemId,
                Tier = entity.Tier,
                Name = entity.Name,
                Type = entity.Type,
                Rarity = entity.Rarity,
                Quantity = entity.Quantity,
                Attack = entity.Attack,
                Defense = entity.Defense,
                SpecialEffect = entity.SpecialEffect,
                AmplifiedDamage = entity.AmplifiedDamage
            };

            

            return loot;
        }

        public static LootItemEntity CreateLootItemEntity(LootItem loot)
        {
            var entity = new LootItemEntity
            {
                Tier = loot.Tier,
                Name = loot.Name,
                Type = loot.Type,
                Rarity = loot.Rarity,
                Quantity = loot.Quantity,
                Attack = loot.Attack,
                Defense = loot.Defense,
                SpecialEffect = loot.SpecialEffect,
                AmplifiedDamage = loot.AmplifiedDamage,
            };

            return entity;
        }
    }
}