using EngineeredAngel.Database.DbServices;
using Godot;
using System;
using System.Collections.Generic;

namespace EngineeredAngel.Services
{
    public partial class LevelUpService : Node
    {
        private AnimatedSprite2D _levelUpAnimation;
        private readonly PlayerDataRepository _playerDataRepository = new();
        private Player _zikky;

        private Queue<int> _pendingLevelUps = new();
        private LevelUpPopup _popup;

        public async void CheckLevelUp(int experience, Player zikky)
        {
            _zikky = zikky;
            _levelUpAnimation = _zikky.GetNode<AnimatedSprite2D>("LevelUpAnimation");
            _popup = _zikky.GetNode<LevelUpPopup>("LevelUpPopup");

            if (!_popup.IsConnected(LevelUpPopup.SignalName.StatSelected, new Callable(this, nameof(OnStatChosen))))
                _popup.Connect(LevelUpPopup.SignalName.StatSelected, new Callable(this, nameof(OnStatChosen)));

            _zikky.CharacterStats.Experience += experience;

            await _playerDataRepository.UpdatePlayerStatsAsync(_zikky.CharacterStats);

            int expForNext = GetExpForNextLevel(_zikky.CharacterStats.Level);

            if (_zikky.CharacterStats.Experience >= expForNext)
            {
                _zikky.CharacterStats.Experience -= expForNext;
                _zikky.CharacterStats.Level++;
                _levelUpAnimation.Play();

                _popup.ShowPopup();
            }
        }


        private async void OnStatChosen(string statName)
        {
            switch (statName)
            {
                case "HP":
                    _zikky.CharacterStats.MaxHP += 10;
                    _zikky.CharacterStats.HP = _zikky.CharacterStats.MaxHP;
                    break;
                case "Strength":
                    _zikky.CharacterStats.Strength += 2;
                    break;
                case "Defense":
                    _zikky.CharacterStats.Defense += 2;
                    break;
                case "Intelligence":
                    _zikky.CharacterStats.Intelligence += 2;
                    break;
            }

            await _playerDataRepository.UpdatePlayerStatsAsync(_zikky.CharacterStats);

            GD.Print($"[LevelUp] Lvl {_zikky.CharacterStats.Level} - Chose: {statName}");
        }

        private int GetExpForNextLevel(int level)
        {
            const int baseExp = 100;
            const double scaleFactor = 2.2;
            return (int)(baseExp * Math.Pow(scaleFactor, level - 1));
        }
    }
}
