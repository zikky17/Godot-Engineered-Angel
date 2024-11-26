using EngineeredAngel.Interfaces;

namespace EngineeredAngel.Loot
{
    public class LootItem : IWeapon
    {
        public int Tier { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public float DropChance { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string SpecialEffect { get; set; }
        public int AmplifiedDamage { get; set; }

        public LootItem(string name, string type, int quantity, float dropChance, int tier, int attack, int defense, string specialEffect, int amplifiedDamage)
        {
            Name = name;
            Type = type;
            Quantity = quantity;
            DropChance = dropChance;
            Tier = tier;
            Attack = attack;
            Defense = defense;
            SpecialEffect = specialEffect;
            AmplifiedDamage = amplifiedDamage;
        }

        public LootItem()
        {
            
        }


        public LootItem(string name, string type, int quantity, float dropChance)
        {
            Name = name;
            Type = type;
            Quantity = quantity;
            DropChance = dropChance;
        }
    }
}
