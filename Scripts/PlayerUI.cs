using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Services;
using Godot;

public partial class PlayerUI : CanvasLayer
{
    private Zikky zikky;
    private Label levelLabel;
    private Label goldLabel;
    private Label healthLabel;

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

        zikky = GetNode<Zikky>("../Zikky");
        levelLabel = GetNode<Label>("Panel/Main_Stats/Level");
        goldLabel = GetNode<Label>("Panel/Main_Stats/Gold");
        healthLabel = GetNode<Label>("Panel/Main_Stats/Health");

        _strengthLabel = GetNode<Label>("Panel/Attributes/Strength");
        _defenceLabel = GetNode<Label>("Panel/Attributes/Defence");
        _intelligenceLabel = GetNode<Label>("Panel/Attributes/Intelligence");
        _maxHpLabel = GetNode<Label>("Panel/Attributes/Max_HP");
        _experienceLabel = GetNode<Label>("Panel/Attributes/Experience");

        levelLabel.SelfModulate = new Color("#f542c5");
        goldLabel.SelfModulate = new Color("#c4ae04");
        healthLabel.SelfModulate = new Color("#0bfc03");

        _strengthLabel.SelfModulate = new Color("#71eef0");
        _defenceLabel.SelfModulate = new Color("#71eef0");
        _intelligenceLabel.SelfModulate = new Color("#71eef0");
        _maxHpLabel.SelfModulate = new Color("#71eef0");
        _experienceLabel.SelfModulate = new Color("#71eef0");


        UpdateUI();
    }

    private void OnLevelUpOccurred(int newLevel, int experience)
    {
        UpdateUI();
    }

    public override void _Process(double delta)
    {

        if (zikky == null || zikky.CharacterStats == null)
        {
            GD.PrintErr("Zikky or its CharacterStats is null. Skipping UI update.");
            return;
        }

        health = zikky.CharacterStats.HP;
        gold = zikky.CharacterStats.Gold;
        level = zikky.CharacterStats.Level;
        experience = zikky.CharacterStats.Experience;

        maxHp = zikky.CharacterStats.MaxHP;
        strength = zikky.CharacterStats.Strength;
        defence = zikky.CharacterStats.Defense;
        intelligence = zikky.CharacterStats.Intelligence;
        
        if (health >= 100)
            healthLabel.SelfModulate = new Color("#0bfc03");
        if (health <= 100)
            healthLabel.SelfModulate = new Color("#0bfc03");
        if (health <= 70)
            healthLabel.SelfModulate = new Color("#dffc03");
        if (health <= 50)
            healthLabel.SelfModulate = new Color("#fc6b03");
        if (health <= 25)
            healthLabel.SelfModulate = new Color("#fc0303");

        UpdateUI();
    }

    private void UpdateUI()
    {

        levelLabel.Text = $"Level: {level}";
        goldLabel.Text = $"Gold: {gold}";
        healthLabel.Text = $"Health: {health}";

        _experienceLabel.Text = $"Experience: {experience}";
        _strengthLabel.Text = $"Strength: {strength}";
        _defenceLabel.Text = $"Defense: {defence}";
        _intelligenceLabel.Text = $"Intelligence: {intelligence}";
        _maxHpLabel.Text = $"Max HP: {maxHp}";
    }
}
