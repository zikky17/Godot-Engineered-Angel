using System;

namespace EngineeredAngel.Stats
{
    public class PlayerStats
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Gold { get; set; }

        public PlayerStats(int hp, int maxHp, int strength, int defense, int gold)
        {
            HP = hp;
            MaxHP = maxHp;
            Strength = strength;
            Defense = defense;
            Gold = gold;
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
