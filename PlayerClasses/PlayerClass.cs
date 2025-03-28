using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.PlayerClasses
{
    public class PlayerClass
    {
        public int Id { get; set; }
        public int MaxHealth { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }

        public virtual void AlloccateStatPoints()
        {

        }
    }
}
