using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerClasses.Wizard
{
    public class Wizard : PlayerClass
    {
        public override void AlloccateStatPoints()
        {
            MaxHealth = 90;
            Strength = 2;
            Agility = 1;
            Defence = 3;
            Intelligence = 5;
        }
    }
}
