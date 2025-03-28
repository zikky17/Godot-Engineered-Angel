using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Database.Models;
using EngineeredAngel.Models.Player;
using EngineeredAngel.PlayerClasses;
using EngineeredAngel.PlayerClasses.Rogue;
using EngineeredAngel.PlayerClasses.Warrior;
using EngineeredAngel.PlayerClasses.Wizard;
using Godot;
using System.Threading.Tasks;

public partial class StartScreen : Control
{
    private LineEdit _nameInput;
    private readonly PlayerDataRepository _playerDataRepository = new PlayerDataRepository();

    public override void _Ready()
    {
        _nameInput = GetNode<LineEdit>("NameInput");
    }

    public void _OnRoguePressed() => CreatePlayer(new Rogue());
    public void _OnWarriorPressed() => CreatePlayer(new Warrior());
    public void _OnWizardPressed() => CreatePlayer(new Wizard());
    public void _OnStartPressed() => LoadMainGame();

    private async void CreatePlayer(PlayerClass playerClass)
    {
        string playerName = _nameInput.Text.Trim();

        playerClass.AlloccateStatPoints();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            GD.PrintErr("Spelarnamn saknas!");
            return;
        }

        var profile = new PlayerProfileModel
        {
            PlayerName = playerName,
            PlayerClass = playerClass,
            CurrentHp = playerClass.MaxHealth,
            MaxHealth = playerClass.MaxHealth,
            Strength = playerClass.Strength,
            Defence = playerClass.Defence,
            Intelligence = playerClass.Intelligence,
            Agility = playerClass.Agility
        };

        await SaveProfileToDatabase(profile);
    }

    private async Task SaveProfileToDatabase(PlayerProfileModel profile)
    {
        var newPlayerData = new GamePlayerEntity
        {
            PlayerName = profile.PlayerName,
            Level = 1,
            CurrentHP = profile.CurrentHp,
            MaxHealth = profile.MaxHealth,
            Strength = profile.Strength,
            Defence = profile.Defence,
            Intelligence = profile.Intelligence,
            Agility = profile.Agility,
            Gold = 0,
            Experience = 0
        };

        await _playerDataRepository.SavePlayerDataAsync(newPlayerData);
    }

    private void LoadMainGame()
    {
        GetTree().ChangeSceneToFile("res://Scenes/start_town.tscn");
    }
}
