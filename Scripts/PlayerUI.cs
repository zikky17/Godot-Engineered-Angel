using Godot;
using System;

public partial class PlayerUI : CanvasLayer
{
    private Zikky zikky;
    private Label goldLabel;
    private Label nameLabel;
    private Label healthLabel;
    private int gold;
    private string name;
    private int health;

    public override void _Ready()
    {

        zikky = GetNode<Zikky>("../Zikky");
        goldLabel = GetNode<Label>("Panel/GridContainer/Gold");
        nameLabel = GetNode<Label>("Panel/GridContainer/Name");
        healthLabel = GetNode<Label>("Panel/GridContainer/Health");

        goldLabel.SelfModulate = new Color(1, 0.84f, 0);
        nameLabel.SelfModulate = new Color(1, 1, 1); 
        healthLabel.SelfModulate = new Color(1, 0, 0); 


        UpdateUI();
    }

    public override void _Process(double delta)
    {
        health = zikky.CharacterStats.HP;
        gold = zikky.CharacterStats.Gold;
        UpdateUI();
    }

    private void UpdateUI()
    {
        goldLabel.Text = $"Gold: {gold}";
        nameLabel.Text = "Name: Zikky";
        healthLabel.Text = $"Health: {health}";
    }
}
