using EngineeredAngel.Enums;
using System;

namespace EngineeredAngel.Loot.Weapons
{
    public class IronSword : LootItem
    {
        public LootItem ReturnWeaponData()
        {
            var randomAttack = new Random();
            int attackValue = randomAttack.Next(1, 4);

            var randomDefense = new Random();
            int defenseValue = randomDefense.Next(1, 4);

            var newWeapon = new IronSword
            {
                Name = "IronSword",
                Type = "Weapon",
                Tier = 10,
                Attack = attackValue,
                Defense = defenseValue,              
                Rarity = GenerateRarityCommonOrRareorEpic(),
                SpecialEffect = null,
                AmplifiedDamage = 0        
            };

            if (Rarity == "Rare")
            {
                newWeapon.Attack += 2;
                newWeapon.Defense += 2;
            }

            if (Rarity == "Epic")
            {
                newWeapon.Attack += 3;
                newWeapon.Defense += 3;
                newWeapon.SpecialEffect = "Frosted Strike";
                newWeapon.AmplifiedDamage += 2;
            }


            return newWeapon;
        }

        public string GenerateRarityCommonOrRareorEpic()
        {
            var random = new Random();
            int chance = random.Next(1, 101);

            if (chance <= 10)
            {
                return "Rare";
            }
            if (chance == 100)
            {
                return "Epic";
            }
            else
            {
                return "Common";
            }
        }
    }
}
