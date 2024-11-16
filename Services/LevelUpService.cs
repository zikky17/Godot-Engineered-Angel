using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Stats;
using Godot;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading.Tasks;

namespace EngineeredAngel.Services
{
    public partial class LevelUpService : Node
    {

        private readonly PlayerDataRepository _playerDataRepository = new();
        private Zikky _zikky;

        public async void CheckLevelUp(int experience, Zikky zikky)
        {
            _zikky = zikky;
            await _playerDataRepository.UpdatePlayerExperienceAsync(experience);

            while (_zikky.CharacterStats.Experience >= GetExpForNextLevel(_zikky.CharacterStats.Level))
            {
                var expNeededForNextLevel = GetExpForNextLevel(_zikky.CharacterStats.Level);
                GD.Print($"Experience needed for next level: {expNeededForNextLevel}");

                _zikky.CharacterStats.Experience -= expNeededForNextLevel;

                _zikky.CharacterStats.Level++;
                ApplyLevelUpBonus();
            }
        }

        private int GetExpForNextLevel(int level)
        {
            const int baseExp = 100;

            const double scaleFactor = 2.2;

            return (int)(baseExp * Math.Pow(scaleFactor, level - 1));
        }

        private async void ApplyLevelUpBonus()
        {
            _zikky.CharacterStats.MaxHP += 10;
            _zikky.CharacterStats.Strength += 2;
            _zikky.CharacterStats.Defense += 1;
            _zikky.CharacterStats.Intelligence += 1;

            await _playerDataRepository.UpdatePlayerLevelAndStatsAsync(_zikky.CharacterStats.Level, 10, 2, 1, 1);
            GD.Print($"Leveled up to {_zikky.CharacterStats.Level}!");
            GD.Print("New stats:");
            GD.Print($"- HP: {_zikky.CharacterStats.MaxHP}");
            GD.Print($"- Strength: {_zikky.CharacterStats.Strength}");
            GD.Print($"- Defense: {_zikky.CharacterStats.Defense}");
            GD.Print($"- Intelligence: {_zikky.CharacterStats.Intelligence}");
        }
    }
}
