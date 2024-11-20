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

        public async Task SaveInventoryAsync(PlayerInventoryEntity entity)
        {
            try
            {
                var inventory = await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
                if (inventory != null)
                {
                    _gameDbContext.Inventory.Update(entity);
                }
                else
                {
                    _gameDbContext.Inventory.Add(entity);
                }

                await _gameDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error saving inventory: {ex.Message}");
            }
        }

        public async Task AddItemToInventory(LootItemEntity loot)
        {
            try
            {
                var inventory = await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
                if (inventory != null)
                {
                    loot.InventoryId = inventory.InventoryId;
                    inventory.LootItems.Add(loot);

                    await _gameDbContext.SaveChangesAsync();
                    GD.Print($"Item {loot.Name} added to inventory.");
                }
                else
                {
                    GD.PrintErr("No inventory found to add item to.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error adding item to inventory: {ex.Message}");
            }
        }

        public async Task<PlayerInventoryEntity> GetPlayerInventoryAsync()
        {
            return await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
        }
    }
}
