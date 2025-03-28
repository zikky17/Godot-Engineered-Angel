using Godot;
using System;

public partial class BackToStartTown : Area2D
{
    [Export] public string TargetScenePath { get; set; } = "res://Scenes/start_town.tscn";

    public override void _Ready()
    {
        Connect(SignalName.BodyEntered, new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            GD.Print($"Player entered the door. Loading scene: {TargetScenePath}");
            CallDeferred(nameof(ChangeScene), player);
        }
    }

    private void ChangeScene(Player player)
    {
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

        var globalState = GetNode<GlobalState>("/root/GlobalState");

        var newPlayer = GetTree().CurrentScene.GetNode<Player>("Zikky");

        if (newPlayer != null)
        {
            GD.Print($"Restoring player position: {globalState.LastPlayerPosition}");
            newPlayer.Position = globalState.LastPlayerPosition; 
        }
    }
}
