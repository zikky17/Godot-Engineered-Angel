using EngineeredAngel.Database.DbServices;
using Godot;

public partial class Loot : Node2D
{
    [Export] public new string Name { get; set; }
    [Export] public string Type { get; set; }
    [Export] public int Quantity { get; set; }

    private Area2D _area2D;
    private readonly PlayerInventoryRepository _playerInventoryRepository = new();

    public override void _Ready()
    {
        _area2D = GetNode<Area2D>("Area2D");
        _area2D.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));

    }

    private void OnBodyEntered(Node body)
    {
        if (body is Zikky player)
        {
            player.AddToInventory(Name, Type, Quantity);
            GD.Print($"Player picked up {Quantity} x {Name}");
            AddLootToDatabase();
            GD.Print($"Emitting signal: {nameof(GameManager.ItemPickedUpEventHandler)} with Name={Name}, Type={Type}, Quantity={Quantity}");
            GameManager.Instance.EmitSignal(nameof(GameManager.ItemPickedUpEventHandler), Name, Type, Quantity);
            GD.Print("Signal emitted.");
            QueueFree();
        }
    }

    private async void AddLootToDatabase()
    {

        await _playerInventoryRepository.GetOrCreateInventoryAsync();
        await _playerInventoryRepository.AddLootToDatabase(Name, Type, Quantity, 1);
    }
}
