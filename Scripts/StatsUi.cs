using Godot;
using System;

public partial class StatsUi : CanvasLayer
{

    private Zikky zikky;
    private Label strengthLabel;
    private Label defenceLabel;
    private Label intelligenceLabel;
    private Label maxHpLabel;
    private Label experienceLabel;

    private int strength;
    private int defence;
    private int intelligence;
    private int maxHp;
    private int experience; 


    public override void _Ready()
	{
        zikky = GetNode<Zikky>("../Zikky");
        strengthLabel = GetNode<Label>("Panel/GridContainer/Strength");
        defenceLabel = GetNode<Label>("Panel/GridContainer/Defence");
        intelligenceLabel = GetNode<Label>("Panel/GridContainer/Intelligence");
        maxHpLabel = GetNode<Label>("Panel/GridContainer/Max_HP");
        experienceLabel = GetNode<Label>("Panel/GridContainer/Experience");


        UpdateUI();
    }

	public override void _Process(double delta)
	{
        strength = zikky.CharacterStats.Strength;
        defence = zikky.CharacterStats.Defense;
        intelligence = zikky.CharacterStats.Intelligence;
        maxHp = zikky.CharacterStats.MaxHP;
        experience = zikky.CharacterStats.Experience;



        UpdateUI();
    }

    private void UpdateUI()
    {
        strengthLabel.Text = $"Strength: {strength}";
        defenceLabel.Text = $"Defence: {defence}";
        intelligenceLabel.Text = $"Intelligence: {intelligence}";
        maxHpLabel.Text = $"Max HP: {maxHp}";
        experienceLabel.Text = $"Experience: {experience}";
    }
}
