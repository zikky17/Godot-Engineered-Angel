using EngineeredAngel.Database.Context;
using EngineeredAngel.Database.Models;
using EngineeredAngel.Factories;
using EngineeredAngel.Loot;
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



        public async Task AddLootToDatabase(LootItem item, int inventoryId)
        {
            if(item.Type != "Weapon" && item.Type != "Armor")
            {
                var existingLoot = _gameDbContext.LootItems
              .FirstOrDefault(loot => loot.Name == item.Name && loot.Type == item.Type && loot.InventoryId == inventoryId);

                if (existingLoot != null)
                {
                    existingLoot.Quantity += item.Quantity;
                    GD.Print($"Updated existing loot: {item.Name} to Quantity = {existingLoot.Quantity}");
                }
                else
                {
                    var newLoot = new LootItemEntity
                    {
                        Name = item.Name,
                        Type = item.Type,
                        Rarity = item.Rarity.ToString(),
                        Quantity = item.Quantity,
                        Attack = item.Attack,
                        Defense = item.Defense,
                        SpecialEffect = item.SpecialEffect,
                        AmplifiedDamage = item.AmplifiedDamage,
                        InventoryId = inventoryId
                    };
                    await _gameDbContext.LootItems.AddAsync(newLoot);
                    GD.Print($"Added new loot: {item.Name} with Rarity = {item.Rarity}");
                }

            
            }
            else
            {
                var newLoot = new LootItemEntity
                {
                    Name = item.Name,
                    Type = item.Type,
                    Rarity = item.Rarity.ToString(),
                    Attack = item.Attack,
                    Defense = item.Defense,
                    SpecialEffect = item.SpecialEffect,
                    AmplifiedDamage = item.AmplifiedDamage,
                    InventoryId = inventoryId
                };
                await _gameDbContext.LootItems.AddAsync(newLoot);
                GD.Print($"Added new loot: {item.Name} with these Stats:");
                GD.Print($"Attack: {item.Attack}");
                GD.Print($"Defense: {item.Defense}");
                GD.Print($"Rarity: {item.Rarity}");
            }

            await _gameDbContext.SaveChangesAsync();
        }
          


        public async Task<PlayerInventoryEntity> GetPlayerInventoryAsync()
        {
            return await _gameDbContext.Inventory.Include(i => i.LootItems).FirstOrDefaultAsync();
        }

        internal void RemoveLootFromDatabase(LootItem loot)
        {

            var existingLoot = _gameDbContext.LootItems.Find(loot.Id);
            if (existingLoot != null)
            {
                _gameDbContext.LootItems.Remove(existingLoot);
                _gameDbContext.SaveChangesAsync();
            }
        }

        public void AddEquippedWeapon(LootItem loot)
        {
            var newLoot = new LootItemEntity
            {
                Name = loot.Name,
                Type = loot.Type,
                Rarity = loot.Rarity.ToString(),
                Quantity = loot.Quantity,
                Attack = loot.Attack,
                Defense = loot.Defense,
                SpecialEffect = loot.SpecialEffect,
                AmplifiedDamage = loot.AmplifiedDamage
            };

            _gameDbContext.EquippedWeapon.Add(newLoot);
        }

        public LootItem LoadEquippedWeapon()
        {
            var itemEquipped = _gameDbContext.EquippedWeapon.FirstOrDefault();
            var weaponModel = LootFactory.CreateLootItem(itemEquipped);
            return weaponModel;
        }
    }
}
