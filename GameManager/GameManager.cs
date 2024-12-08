using Godot;

public partial class GameManager : Node
{
    [Signal]
    public delegate void ItemPickedUpEventHandler(
     string name,
     string type,
     int quantity,
     string rarity,
     int attack,
     int defense,
     int tier,
     string specialEffect,
     int amplifiedDamage
 );

    public static GameManager Instance;
    private Control _inventoryUi;
    private Control _statsUi;

    public override void _Ready()
    {
        Instance = this;
        _inventoryUi = GetNode<Control>("../Zikky/CharacterMenus/InventoryUI");
        _statsUi = GetNode<Control>("../Zikky/CharacterMenus/PlayerUI"); 
        _inventoryUi.Visible = false;
        _statsUi.Visible = false;
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

    public void ToggleStats()
    {
        _statsUi.Visible = !_statsUi.Visible;
    }



    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_inventory"))
        {
            ToggleInventory();
        } 
        
        if (Input.IsActionJustPressed("toggle_stats"))
        {
            ToggleStats();
        }
    }
}
