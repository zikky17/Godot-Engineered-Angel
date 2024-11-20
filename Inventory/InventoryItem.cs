using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeredAngel.Inventory
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public string Type { get; set; } 
        public int Quantity { get; set; }

        public InventoryItem(string name, string type, int quantity)
        {
            Name = name;
            Type = type;
            Quantity = quantity;
        }
    }

}
