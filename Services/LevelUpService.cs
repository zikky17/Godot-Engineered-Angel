using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Stats;
using Godot;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading.Tasks;

namespace EngineeredAngel.Services
{
    public partial class LevelUpService : Node
    {
        [Signal]
        public delegate void LevelUpOccurredEventHandler(int newLevel, int experience);

        private readonly PlayerDataRepository _playerDataRepository = new();
        private Zikky _zikky;


        public async void AddExperience(int experience, Zikky zikky)
        {
            _zikky = zikky;
            _zikky.CharacterStats.Experience += experience;
            CheckLevelUp();

            await _playerDataRepository.UpdatePlayerExperienceAsync(experience);
        }

        private async void CheckLevelUp()
        {
            var level = _zikky.CharacterStats.Level;
            var expNeededForNextLevel = GetExpForNextLevel(level);

            if (_zikky.CharacterStats.Experience >= expNeededForNextLevel)
            {
                _zikky.CharacterStats.Level++;
                _zikky.CharacterStats.Experience = 0;
                EmitSignal(nameof(LevelUpOccurredEventHandler), _zikky.CharacterStats.Level, _zikky.CharacterStats.Experience);
                ApplyLevelUpBonus();

                await _playerDataRepository.UpdatePlayerLevelAsync(_zikky.CharacterStats.Level);
            }
        }

        private int GetExpForNextLevel(int level)
        {
            return 100 + (level - 1) * 125;
        }

        private async void ApplyLevelUpBonus()
        {
            _zikky.CharacterStats.MaxHP += 10;
            _zikky.CharacterStats.Strength += 2;
            _zikky.CharacterStats.Defense += 1;
            _zikky.CharacterStats.Intelligence += 1;

            await _playerDataRepository.UpdatePlayerLevelUpStatsAsync(1, 10, 2, 1, 1);
            GD.Print($"Leveled up to {_zikky.CharacterStats.Level}! New stats - HP: {_zikky.CharacterStats.MaxHP}, Strength: {_zikky.CharacterStats.Strength}, Defense: {_zikky.CharacterStats.Defense}");
        }
    }
}
