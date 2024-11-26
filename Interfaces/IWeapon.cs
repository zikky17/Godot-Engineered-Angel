using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.Interfaces
{
    public interface IWeapon
    {
        public int Tier { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        [AllowNull]
        public string SpecialEffect { get; set; }
        [AllowNull]
        public int AmplifiedDamage { get; set; }
    }
}
