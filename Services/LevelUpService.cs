using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Stats;
using Godot;
using System.Collections.Generic;

namespace EngineeredAngel.Services
{
    public class LevelUpService
    {
        public int Experience { get; private set; }
        public int Level { get; private set; } = 1;
        public int ExpForNextLevel { get; private set; }
        public PlayerStats PlayerStats { get; set; }

        private readonly List<int> experienceRequirements;
        private readonly PlayerDataRepository _playerDataRepository = new();
        private bool isCheckingLevelUp = false;

        public LevelUpService()
        {
            experienceRequirements = GenerateExperienceRequirements(30, 100, 1.25);
            ExpForNextLevel = experienceRequirements[Level - 1];
        }

        private List<int> GenerateExperienceRequirements(int maxLevel, int baseExp, double multiplier)
        {
            var requirements = new List<int> { baseExp };
            for (int i = 1; i < maxLevel; i++)
            {
                var previousExp = requirements[i - 1];
                var newExp = (int)(previousExp * multiplier);
                requirements.Add(newExp);
                GD.Print($"Level {i + 1} requires {newExp} experience");
            }
            return requirements;
        }

        public void GetExperience(int experience)
        {
            Experience += experience;
            CheckLevelUp();
        }

        private async void CheckLevelUp()
        {
            if (isCheckingLevelUp) return;
            isCheckingLevelUp = true;

            while (Level < experienceRequirements.Count && Experience >= ExpForNextLevel)
            {
                Experience -= ExpForNextLevel;
                Level += 1;
                PlayerStats.Level = Level;
                ExpForNextLevel = experienceRequirements[Level - 1];
                await _playerDataRepository.UpdatePlayerLevelAsync(Level);
                ApplyLevelUpBonus();
            }

            isCheckingLevelUp = false;
        }

        private void ApplyLevelUpBonus()
        {
            PlayerStats.MaxHP += 10;
            PlayerStats.Strength += 2;
            PlayerStats.Defense += 1;
            GD.Print($"Leveled up to {Level}! New stats - HP: {PlayerStats.MaxHP}, Strength: {PlayerStats.Strength}, Defense: {PlayerStats.Defense}");
        }
    }
}
