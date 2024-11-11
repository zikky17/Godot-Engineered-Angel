using Godot;
using System;

public partial class HealthPotion : Area2D
{

    [Export] public int HealAmount = 20;

    public override void _Ready()
    {
        Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnPickUp)));
        AddToGroup("HealthConsumable");
    }

    private void OnPickUp(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            var player = (Zikky)body;
            player.Heal(HealAmount);
            QueueFree();
        }
    }

}
