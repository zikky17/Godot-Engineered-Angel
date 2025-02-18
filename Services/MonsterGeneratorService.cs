using EngineeredAngel.Interfaces;
using EngineeredAngel.Monsters;
using System;

namespace EngineeredAngel.Services
{
    public class MonsterGeneratorService
    {
        public IMonster RetrieveMonsterProfile(string monsterName)
        {
            if (monsterName.Contains("Gorgon"))
            {
                return new Gorgon();
            }
            else if (monsterName.Contains("HorrorWasp"))
            {
                return new HorrorWasp();
            }

            return null;
        }
    }
}
