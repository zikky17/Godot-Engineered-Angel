using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Database.Models;
using EngineeredAngel.Stats;
using Godot;

namespace EngineeredAngel.Services
{
    public class RewardService
    {
        private readonly Zikky _zikky;
        private readonly LevelUpService _levelUpService = new();
        private readonly PlayerDataRepository _playerDataRepository = new();

        public RewardService(Zikky zikky, LevelUpService levelUpService)
        {
            _zikky = zikky;
            _levelUpService = levelUpService;
        }

        public async void GrantRewards(int gold, int experience)
        {
            _zikky.CharacterStats.Gold += gold;
            _levelUpService.GetExperience(experience);

            await _playerDataRepository.UpdatePlayerGoldAsync(gold);
            await _playerDataRepository.UpdatePlayerExperienceAsync(experience);
            GD.Print($"Zikky's gold is now {_zikky.CharacterStats.Gold}");
        }
    }
}
