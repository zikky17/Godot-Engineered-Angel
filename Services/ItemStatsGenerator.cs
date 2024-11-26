using EngineeredAngel.Loot;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;

namespace EngineeredAngel.Services
{
    public class ItemStatsGenerator
    {

        public string Common { get; set; } = "Common";
        public string Rare { get; set; } = "Rare";
        public string Epic { get; set; } = "Epic";
        public string Legendary { get; set; } = "Legendary";
        public string Godly { get; set; } = "Godly";
        public string Immortal { get; set; } = "Immortal";



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

            string rarity = GenerateRarityCommonOrRare();

            var newWeapon = new LootItem
            {
                Name = name,
                Type = type,
                Quantity = quantity,
                Rarity = rarity,
                Tier = 10,
                Attack = attackValue,
                Defense = defenseValue,
                SpecialEffect = null,
                AmplifiedDamage = 0
            };

            if (rarity == Rare)
            {
                newWeapon.Attack += 2;
                newWeapon.Defense += 2;
            }


            return newWeapon;
        }

        private string GenerateRarityCommonOrRare()
        {
            var random = new Random();
            int chance = random.Next(1, 101); 

            if (chance <= 10)
            {
                return Rare;
            }
            else
            {
                return Common;
            }
        }
    }
}
