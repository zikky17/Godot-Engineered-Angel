using EngineeredAngel.Loot;
using EngineeredAngel.Loot.Weapons;
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
                    var item = new IronSword();
                    var sword = item.ReturnWeaponData();
                    return sword;
                default:
                    throw new ArgumentException($"Weapon {name} is not recognized.");
            }
        }   
    }
}
