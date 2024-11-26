using EngineeredAngel.Loot;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;

namespace EngineeredAngel.Services
{
    public class ItemStatsGenerator
    {

        public LootItem ApplyStatsForWeapon(string name, string type, int quantity)
        {
            switch (name)
            {
                case "Iron Sword":
                    var item = GenerateStatsForIronSword(name, type, quantity);
                    return item;
                default:
                    throw new ArgumentException($"Weapon {name} is not recognized.");
            }
        }

        public LootItem GenerateStatsForIronSword(string name, string type, int quantity)
        {

            var randomAttack = new Random();
            int attackValue = randomAttack.Next(1, 4);

            var randomDefense = new Random();
            int defenseValue = randomDefense.Next(1, 4);

            var newWeapon = new LootItem
            {
                Name = name,
                Type = type,
                Quantity = quantity,
                Tier = 10,
                Attack = attackValue,
                Defense = defenseValue,
                SpecialEffect = null,
                AmplifiedDamage = 0
            };

            return newWeapon;
        }
    }
}
