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
     int amplifiedDamage);

    [Signal]
    public delegate void QuestAcceptedEventHandler(
    string questName,
    string questText);

    public static GameManager Instance;
    private Control _inventoryUi;
    private Control _statsUi;
    private Control _questsUi;

    public override void _Ready()
    {
        Instance = this;
        _inventoryUi = GetNode<Control>("../CharacterMenus/InventoryUI");
        _statsUi = GetNode<Control>("../CharacterMenus/PlayerUI");
        _questsUi = GetNode<Control>("../CharacterMenus/QuestMenu"); 
        _inventoryUi.Visible = false;
        _statsUi.Visible = false;
        _questsUi.Visible = false;
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

        if (!HasSignal(nameof(QuestAcceptedEventHandler)))
        {
            AddUserSignal(nameof(QuestAcceptedEventHandler));
            GD.Print("Signal 'QuestAcceptedEventHandler' manually registered.");
        }
        else
        {
            GD.Print("Signal 'QuestAcceptedEventHandler' is already registered.");
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

    public void ToggleQuestMenu()
    {
        _questsUi.Visible = !_questsUi.Visible;
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

        if (Input.IsActionJustPressed("toggle_quests"))
        {
            ToggleQuestMenu();
        }
    }
}
