using Godot;
using System;

public partial class PlayerUI : CanvasLayer
{
    private Zikky zikky;
    private Label levelLabel;
    private Label goldLabel;
    private Label healthLabel;

    private int level;
    private int gold;
    private string name;
    private int health;

    public override void _Ready()
    {

        zikky = GetNode<Zikky>("../Zikky");
        levelLabel = GetNode<Label>("Panel/GridContainer/Level");
        goldLabel = GetNode<Label>("Panel/GridContainer/Gold");
        healthLabel = GetNode<Label>("Panel/GridContainer/Health");

        levelLabel.SelfModulate = new Color("#fc3503");
        goldLabel.SelfModulate = new Color("#fc3503");
        healthLabel.SelfModulate = new Color("#0bfc03");


        UpdateUI();
    }

    public override void _Process(double delta)
    {
        level = zikky.CharacterStats.Level;
        health = zikky.CharacterStats.HP;
        if (health <= 100)
            healthLabel.SelfModulate = new Color("#0bfc03");
        if (health <= 70)
            healthLabel.SelfModulate = new Color("#dffc03");
        if (health <= 50)
            healthLabel.SelfModulate = new Color("#fc6b03");
        if (health <= 25)
            healthLabel.SelfModulate = new Color("#fc0303");
        gold = zikky.CharacterStats.Gold;
        UpdateUI();
    }

    private void UpdateUI()
    {
        levelLabel.Text = $"Level: {level}";
        goldLabel.Text = $"Gold: {gold}";
        healthLabel.Text = $"Health: {health}";
    }
}
