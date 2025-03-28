﻿using EngineeredAngel.Database.DbServices;
using EngineeredAngel.PlayerClasses;
using System;
using System.Threading.Tasks;

namespace EngineeredAngel.Stats
{
    public class PlayerStats
    {
        public int Level { get; set; }
        public string PlayerName { get; set; }
        public string ClassName { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Gold { get; set; }
        public int VoidScraps { get; set; }
        public int Experience { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }

        public PlayerStats()
        {

        }

        public PlayerStats(
            int level, 
            string playerName,
            string className,
            int hp,
            int maxHp, 
            int strength, 
            int defense, 
            int gold, 
            int experience, 
            int intelligence,
            int agility)
        {
            Level = level;
            PlayerName = playerName;
            ClassName = className;
            HP = hp;
            MaxHP = maxHp;
            Strength = strength;
            Defense = defense;
            Gold = gold;
            Experience = experience;
            Intelligence = intelligence;    
            Agility = agility;
        }

        public int CalculateDamage(int incomingDamage)
        {
            int reducedDamage = Math.Max(0, incomingDamage - Defense);
            HP = Math.Max(0, HP - reducedDamage);
            return reducedDamage;
        }

        public void RestoreHealth(int amount)
        {
            HP = Math.Min(MaxHP, HP + amount);
        }

        public int DealDamage()
        {
            var baseDamage = 2;
            var damage = baseDamage + Strength;
            return damage;
        }
    }
}
