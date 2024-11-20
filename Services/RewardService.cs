using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Stats;
using Godot;

namespace EngineeredAngel.Services
{
    public class RewardService
    {
        private readonly Zikky _zikky;
        public readonly LevelUpService _levelUpService;
        private readonly PlayerDataRepository _playerDataRepository = new();

        public RewardService(Zikky zikky, LevelUpService levelUpService)
        {
            _zikky = zikky;
            _levelUpService = levelUpService;
        }

        public async void GrantRewards(int? gold, int? experience)
        {
            if (gold.HasValue)
            {
                _zikky.CharacterStats.Gold += gold.Value;
            }

            if (experience.HasValue)
            {
                _zikky.CharacterStats.Experience += experience.Value;
            }

            await _playerDataRepository.UpdatePlayerStatsAndLevelAsync(gold, experience, null, null, null, null, null);

            if (experience.HasValue)
            {
                _levelUpService.CheckLevelUp(experience.Value, _zikky);
            }

            GD.Print($"Gold: {_zikky.CharacterStats.Gold}, Experience: {_zikky.CharacterStats.Experience}");
        }

    }
}
