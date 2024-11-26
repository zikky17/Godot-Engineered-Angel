using Godot;
using System;

public partial class DoorEntrance : Area2D
{
    [Export] public string TargetScenePath { get; set; } = "res://Scenes/horror_area.tscn";
    [Export] public Vector2 TargetPosition { get; set; } = Vector2.Zero; 

    public override void _Ready()
    {
        Connect(nameof(SignalName.BodyEntered), new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Zikky player)
        {
            GD.Print($"Player entered the door. Loading scene: {TargetScenePath}");

            var nextScene = (PackedScene)GD.Load(TargetScenePath);
            if (nextScene == null)
            {
                GD.PrintErr($"Failed to load scene: {TargetScenePath}");
                return;
            }

            var newScene = nextScene.Instantiate();
            GetTree().Root.AddChild(newScene);

            var currentScene = GetTree().CurrentScene;
            GetTree().CurrentScene = (Node)newScene;
            currentScene.QueueFree();

            if (newScene.HasNode("PlayerStart") && player != null)
            {
                var startPosition = newScene.GetNode<Node2D>("PlayerStart");
                player.GlobalPosition = startPosition.GlobalPosition + TargetPosition;
            }
        }
    }
}
