using EngineeredAngel.Interfaces;
using EngineeredAngel.Loot;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.Monsters
{
    internal class Gorgon : IMonster
    {
        public List<LootItem> LootTable { get; set; } = new List<LootItem>();
        public AudioStream ChargeSound { get; set; }
        public AudioStream DeathSound { get; set; }

        public IMonster ReturnMonsterData()
        {
            return new Gorgon
            {
                LootTable = new List<LootItem>
             {
                    new LootItem("Experience", "Resource", 10, 1.0f),
                    new LootItem("Gold", "Resource", 4, 0.8f),
                    new LootItem("Iron Sword", "Weapon", 1, 1.0f),
                    new LootItem("Healing Potion", "Potion", 1, 0.1f)
             },
                ChargeSound = GD.Load<AudioStream>("res://Assets/SoundEffects/ScarySounds/Gorgon_Laugh.mp3"),
                DeathSound = GD.Load<AudioStream>("res://Assets/SoundEffects/ScarySounds/Gorgon_Death.mp3")
            };        
        }
    }
}
