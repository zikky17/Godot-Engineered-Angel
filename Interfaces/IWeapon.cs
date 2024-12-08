using EngineeredAngel.Enums;
using EngineeredAngel.Loot;
using System.Diagnostics.CodeAnalysis;

namespace EngineeredAngel.Interfaces
{
    public interface IWeapon
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        [AllowNull]
        public string SpecialEffect { get; set; }
        [AllowNull]
        public int AmplifiedDamage { get; set; }
    }
}
