﻿using EngineeredAngel.Database.Context;
using EngineeredAngel.Database.Models;
using EngineeredAngel.Factories;
using EngineeredAngel.Loot;
using EngineeredAngel.Stats;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<PlayerStats> GetPlayerStatsAsync(int playerId)
        {
            var entity = await _gameDbContext.Player.FirstOrDefaultAsync(p => p.Id == playerId);
            return new PlayerStats
            {
                Level = entity.Level,
                HP = entity.CurrentHP,
                MaxHP = entity.MaxHealth,
                Strength = entity.Strength,
                Defense = entity.Defence,
                Gold = entity.Gold,
                Experience = entity.Experience,
                Intelligence = entity.Intelligence
            };
        }

        public async Task SavePlayerDataAsync(GamePlayerEntity player)
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

        public async Task UpdatePlayerStatsAsync(PlayerStats stats)
        {
            try
            {
                using var context = new GameDbContext();
                var player = await context.Player.FirstOrDefaultAsync(p => p.Id == 1);

                if (player != null)
                {
                    player.Level = stats.Level;
                    player.CurrentHP = stats.HP;
                    player.MaxHealth = stats.MaxHP;
                    player.Strength = stats.Strength;
                    player.Defence = stats.Defense;
                    player.Intelligence = stats.Intelligence;
                    player.Experience = stats.Experience;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                GD.Print($"Error saving data: {ex.Message}");
            }
        }


    }
}
