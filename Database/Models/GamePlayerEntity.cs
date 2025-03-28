using EngineeredAngel.PlayerClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.Database.Models
{
    public class GamePlayerEntity
    {
        [Key]
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int CurrentHP { get; set; }
        public int MaxHealth { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
    }
}
