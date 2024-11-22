using EngineeredAngel.Database.Context;
using EngineeredAngel.Database.Models;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EngineeredAngel.Database.DbServices
{
    public class PlayerInventoryRepository
    {
        private readonly GameDbContext _gameDbContext = new GameDbContext();


        public async Task<PlayerInventoryEntity> GetOrCreateInventoryAsync()
        {
            try
            {
                var inventory = await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
                if (inventory != null)
                {
                    return inventory;
                }

                inventory = new PlayerInventoryEntity();
                _gameDbContext.Inventory.Add(inventory);
                await _gameDbContext.SaveChangesAsync();
                GD.Print("New inventory created.");
                return inventory;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error creating or fetching inventory: {ex.Message}");
                return null;
            }
        }



        public async Task AddLootToDatabase(string name, string type, int quantity, int inventoryId)
        {
            var existingLoot = _gameDbContext.LootItems
                .FirstOrDefault(loot => loot.Name == name && loot.Type == type && loot.InventoryId == inventoryId);

            if (existingLoot != null)
            {
                existingLoot.Quantity += quantity;
                GD.Print($"Updated existing loot: {name} to Quantity = {existingLoot.Quantity}");
            }
            else
            {
                var newLoot = new LootItemEntity
                {
                    Name = name,
                    Type = type,
                    Quantity = quantity,
                    InventoryId = inventoryId
                };
                await _gameDbContext.LootItems.AddAsync(newLoot);
                GD.Print($"Added new loot: {name} with Quantity = {quantity}");
            }

           await _gameDbContext.SaveChangesAsync();
        }


        public async Task<PlayerInventoryEntity> GetPlayerInventoryAsync()
        {
            return await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
        }
    }
}
