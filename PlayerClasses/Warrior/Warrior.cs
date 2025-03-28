using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerClasses.Warrior
{
    public class Warrior : PlayerClass
    {

        public override string ClassName => "Warrior";
        public override void AlloccateStatPoints()
        {
            MaxHealth = 100;
            Strength = 5;
            Agility = 1;
            Defence = 3;
            Intelligence = 1;
        }
    }
}
