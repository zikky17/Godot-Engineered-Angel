using EngineeredAngel.Database.Context;
using EngineeredAngel.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EngineeredAngel.Database.DbServices
{
    public class PlayerDataRepository
    {
        private readonly GameDbContext _gameDbContext = new GameDbContext();


        public async Task<GamePlayerEntity> GetPlayerDataAsync(int playerId)
        {
            return await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task AddOrUpdatePlayerDataAsync(GamePlayerEntity player)
        {
            var existingPlayer = await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == 1);

            if (existingPlayer != null)
            {
                existingPlayer.Level = player.Level;
                existingPlayer.CurrentHP = player.CurrentHP;
                existingPlayer.MaxHealth = player.MaxHealth;
                existingPlayer.Strength = player.Strength;
                existingPlayer.Defence = player.Defence;
                existingPlayer.Gold = player.Gold;
                existingPlayer.Experience = player.Experience;
            }
            else
            {
                await _gameDbContext.Player.AddAsync(player);
            }

            await _gameDbContext.SaveChangesAsync();
        }

        public async Task UpdatePlayerGoldAsync(int gold)
        {
            var existingPlayer = await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == 1);

            if (existingPlayer != null)
            {
                existingPlayer.Gold += gold;
                await _gameDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdatePlayerExperienceAsync(int experience)
        {
            var existingPlayer = await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == 1);

            if (existingPlayer != null)
            {
                existingPlayer.Experience += experience;
                await _gameDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdatePlayerLevelAsync(int level)
        {
            var existingPlayer = await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == 1);

            if (existingPlayer != null)
            {
                existingPlayer.Level += level;
                await _gameDbContext.SaveChangesAsync();
            }
        }



    }
}
