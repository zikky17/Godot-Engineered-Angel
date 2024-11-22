using Godot;

public partial class GameManager : Node
{
    [Signal]
    public delegate void ItemPickedUpEventHandler(string itemName, string itemType, int quantity);

    public static GameManager Instance;
    private Control _inventoryUi;

    public override void _Ready()
    {
        Instance = this;
        _inventoryUi = GetNode<Control>("/root/start_area/PlayerUI/PlayerUI/InventoryUI"); 
        _inventoryUi.Visible = false;
        GD.Print("GameManager Instance set.");

        if (!HasSignal(nameof(ItemPickedUpEventHandler)))
        {
            AddUserSignal(nameof(ItemPickedUpEventHandler));
            GD.Print("Signal 'ItemPickedUpEventHandler' manually registered.");
        }
        else
        {
            GD.Print("Signal 'ItemPickedUpEventHandler' is already registered.");
        }


    }

    public void ToggleInventory()
    {
        _inventoryUi.Visible = !_inventoryUi.Visible;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_inventory"))
        {
            ToggleInventory();
        }
    }
}
