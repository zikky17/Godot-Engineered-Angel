using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Services;
using Godot;

public partial class PlayerUI : TextureRect
{
	private Zikky _zikky;
	private Label _levelLabel;
	private Label _goldLabel;
	private Label _healthLabel;

	private Label _experienceLabel;
	private Label _strengthLabel;
	private Label _defenceLabel;
	private Label _intelligenceLabel;
	private Label _maxHpLabel;

	private int health;
	private int gold;
	private int level;
	private int experience;

	private int maxHp;
	private int strength;
	private int defence;
	private int intelligence;

	private readonly PlayerDataRepository _playerDataRepository = new();

	public override void _Ready()
	{

		_zikky = GetNode<Zikky>("../../../Zikky");
		_levelLabel = GetNode<Label>("Main_Stats/Level");
		_healthLabel = GetNode<Label>("Main_Stats/Health");
		_goldLabel = GetNode<Label>("Main_Stats/Gold");

		_strengthLabel = GetNode<Label>("Attributes/Strength");
		_defenceLabel = GetNode<Label>("Attributes/Defence");
		_intelligenceLabel = GetNode<Label>("Attributes/Intelligence");
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

		if (_zikky == null || _zikky.CharacterStats == null)
		{
			GD.PrintErr("Zikky or its CharacterStats is null. Skipping UI update.");
			return;
		}

		health = _zikky.CharacterStats.HP;
		gold = _zikky.CharacterStats.Gold;
		level = _zikky.CharacterStats.Level;
		experience = _zikky.CharacterStats.Experience;

		maxHp = _zikky.CharacterStats.MaxHP;
		strength = _zikky.CharacterStats.Strength;
		defence = _zikky.CharacterStats.Defense;
		intelligence = _zikky.CharacterStats.Intelligence;
		
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

		_levelLabel.Text = $"Level: {level}";
		_goldLabel.Text = $"Gold: {gold}";
		_healthLabel.Text = $"Health: {health}";

		_experienceLabel.Text = $"Experience: {experience}";
		_strengthLabel.Text = $"Strength: {strength}";
		_defenceLabel.Text = $"Defense: {defence}";
		_intelligenceLabel.Text = $"Intelligence: {intelligence}";
		_maxHpLabel.Text = $"Max HP: {maxHp}";
	}
}
