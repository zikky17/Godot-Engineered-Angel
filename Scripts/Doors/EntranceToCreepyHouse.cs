using Godot;
using System;

public partial class EntranceToCreepyHouse : Area2D
{
    [Export] public string TargetScenePath { get; set; } = "res://Scenes/creepy_house.tscn";

    public override void _Ready()
    {
        Connect(SignalName.BodyEntered, new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Zikky player)
        {
            GD.Print($"Player entered the door. Loading scene: {TargetScenePath}");

            GlobalState globalState = GetNode<GlobalState>("/root/GlobalState");
            globalState.LastPlayerPosition = player.Position + new Vector2(0, 32);

            CallDeferred(nameof(ChangeScene), player);
        }
    }

    private void ChangeScene(Zikky player)
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
    }
}
