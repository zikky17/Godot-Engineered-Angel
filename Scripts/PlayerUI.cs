using EngineeredAngel.Database.DbServices;
using EngineeredAngel.PlayerClasses;
using EngineeredAngel.Services;
using Godot;

public partial class PlayerUI : TextureRect
{
	private Player _player;
	private Label _levelLabel;
	private Label _goldLabel;
	private Label _healthLabel;

	private Label _experienceLabel;
	private Label _strengthLabel;
	private Label _defenceLabel;
	private Label _intelligenceLabel;
	private Label _agilityLabel;
	private Label _maxHpLabel;
	private Label _playerNameLabel;
	private Label _playerClassLabel;

	private string playerName;
	private string playerClass;
	private int health;
	private int gold;
	private int level;
	private int experience;

	private int maxHp;
	private int strength;
	private int defence;
	private int intelligence;
	private int agility;

	private readonly PlayerDataRepository _playerDataRepository = new();

	public override void _Ready()
	{

        _player = GetNode<Player>("../../../Player");
		_levelLabel = GetNode<Label>("Main_Stats/Level");
		_playerClassLabel = GetNode<Label>("Main_Stats/PlayerClass");
        _playerNameLabel = GetNode<Label>("Main_Stats/PlayerName");
		_healthLabel = GetNode<Label>("Main_Stats/Health");
		_goldLabel = GetNode<Label>("Main_Stats/Gold");

		_strengthLabel = GetNode<Label>("Attributes/Strength");
		_defenceLabel = GetNode<Label>("Attributes/Defence");
		_intelligenceLabel = GetNode<Label>("Attributes/Intelligence");
		_agilityLabel = GetNode<Label>("Attributes/Agility");
        _maxHpLabel = GetNode<Label>("Attributes/Max_HP");
		_experienceLabel = GetNode<Label>("Attributes/Experience");

		UpdateUI();

	}

	private void OnLevelUpOccurred(int newLevel, int experience)
	{
		UpdateUI();
	}

	public override void _Process(double delta)
	{

		if (_player == null || _player.CharacterStats == null)
		{
			GD.PrintErr("Player or its CharacterStats is null. Skipping UI update.");
			return;
		}

		health = _player.CharacterStats.HP;
		gold = _player.CharacterStats.Gold;
		level = _player.CharacterStats.Level;
		experience = _player.CharacterStats.Experience;
		playerName = _player.CharacterStats.PlayerName;
		playerClass = _player.CharacterStats.ClassName;

		maxHp = _player.CharacterStats.MaxHP;
		strength = _player.CharacterStats.Strength;
		defence = _player.CharacterStats.Defense;
		intelligence = _player.CharacterStats.Intelligence;
		agility = _player.CharacterStats.Agility;	
		
		if (health >= 100)
			_healthLabel.SelfModulate = new Color("#0bfc03");
		if (health <= 100)
			_healthLabel.SelfModulate = new Color("#0bfc03");
		if (health <= 70)
			_healthLabel.SelfModulate = new Color("#dffc03");
		if (health <= 50)
			_healthLabel.SelfModulate = new Color("#fc6b03");
		if (health <= 25)
			_healthLabel.SelfModulate = new Color("#fc0303");

		UpdateUI();
	}

	private void UpdateUI()
	{
		_playerNameLabel.Text = $"Name: {playerName}";
		_playerClassLabel.Text = $"Class: {playerClass}";
        _levelLabel.Text = $"Level: {level}";
		_goldLabel.Text = $"Gold: {gold}";
		_healthLabel.Text = $"Health: {health}";

		_experienceLabel.Text = $"Experience: {experience}";
		_strengthLabel.Text = $"Strength: {strength}";
		_defenceLabel.Text = $"Defense: {defence}";
		_intelligenceLabel.Text = $"Intelligence: {intelligence}";
		_agilityLabel.Text = $"Agility: {agility}";
		_maxHpLabel.Text = $"Max HP: {maxHp}";
	}
}
