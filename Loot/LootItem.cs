namespace EngineeredAngel.Loot
{
    public class LootItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public float DropChance { get; set; }

        public LootItem(string name, string type, int quantity, float dropChance)
        {
            Name = name;
            Type = type;
            Quantity = quantity;
            DropChance = dropChance;
        }
    }
}
