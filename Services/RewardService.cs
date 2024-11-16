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

        public async void GrantRewards(int gold, int experience)
        {
            _zikky.CharacterStats.Gold += gold;
            _zikky.CharacterStats.Experience += experience;
            _levelUpService.CheckLevelUp(experience, _zikky);
            await _playerDataRepository.UpdatePlayerGoldAsync(gold);
            await _playerDataRepository.UpdatePlayerExperienceAsync(experience);
            GD.Print($"Gold: {_zikky.CharacterStats.Gold}, Experience: {_zikky.CharacterStats.Experience}");
        }
    }
}
