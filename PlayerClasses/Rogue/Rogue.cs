using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerClasses.Rogue
{
    public class Rogue : PlayerClass
    {
        public override string ClassName => "Rogue";

        public override void AlloccateStatPoints()
        {
            MaxHealth = 95;
            Strength = 2;
            Agility = 5;
            Defence = 2;
            Intelligence = 1;
        }
    }
}
