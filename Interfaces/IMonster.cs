using EngineeredAngel.Loot;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.Interfaces
{
    public interface IMonster
    {
        List<LootItem> LootTable { get; set; }
        AudioStream ChargeSound { get; set; }
        AudioStream DeathSound { get; set; }

        public IMonster ReturnMonsterData();
        public int DealDamage();
    }
}
