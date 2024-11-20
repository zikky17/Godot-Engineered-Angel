using Godot;

public partial class GameManager : Node
{
    private Control _inventoryUi;

    public override void _Ready()
    {
        _inventoryUi = GetNode<Control>("/root/start_area/PlayerUI/InventoryUI"); 
        _inventoryUi.Visible = false;
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
