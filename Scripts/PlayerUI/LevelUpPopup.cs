using Godot;
using System;

public partial class LevelUpPopup : Control
{
    [Signal] public delegate void StatSelectedEventHandler(string stat);

    public override void _Ready()
    {
        GetNode<Button>("Panel/StatsButtonContainer/HpButton").Pressed += () => OnStatSelected("HP");
        GetNode<Button>("Panel/StatsButtonContainer/StrengthButton").Pressed += () => OnStatSelected("Strength");
        GetNode<Button>("Panel/StatsButtonContainer/DefenseButton").Pressed += () => OnStatSelected("Defense");
        GetNode<Button>("Panel/StatsButtonContainer/IntelligenceButton").Pressed += () => OnStatSelected("Intelligence");
    }

    private void OnStatSelected(string stat)
    {
        EmitSignal(SignalName.StatSelected, stat);
        Hide();
    }

    public void ShowPopup()
    {
        Visible = true;
    }
}
