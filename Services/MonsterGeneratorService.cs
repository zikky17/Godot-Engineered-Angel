using EngineeredAngel.Interfaces;
using EngineeredAngel.Monsters;
using System;

namespace EngineeredAngel.Services
{
    public class MonsterGeneratorService
    {
        public IMonster RetrieveMonsterProfile(string monsterName)
        {
           switch(monsterName)
            {
                case "HorrorWasp":
                        return new HorrorWasp();
                case "Gorgon":
                    return new Gorgon();
            }

            return null;
        }
    }
}
