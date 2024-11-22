using Godot;
using EngineeredAngel.Database.DbServices;

public partial class InventoryUi : Control
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;

    public override void _Ready()
    {
        if (GameManager.Instance != null)
        {
            GD.Print("Connecting signal...");
            GameManager.Instance.Connect(
                nameof(GameManager.ItemPickedUpEventHandler),
                new Callable(this, nameof(OnItemPickedUp))
            );
            GD.Print("Signal connected.");
        }
        else
        {
            GD.Print("GameManager instance is null. Cannot connect signal.");
        }

        _gridContainer = GetNode<GridContainer>("PanelContainer/ScrollContainer/GridContainer");
        _inventoryRepository = new PlayerInventoryRepository();

        UpdateInventoryUI();
    }

    private void OnItemPickedUp(string itemName, string itemType, int quantity)
    {
        GD.Print($"Received Signal: {quantity} x {itemName} ({itemType}) picked up.");
        UpdateItemQuantityInUI(itemName, quantity);
    }

    private void UpdateItemQuantityInUI(string itemName, int quantity)
    {
        GD.Print($"Updating {itemName} with quantity {quantity} in UI...");
        bool itemExists = false;

        foreach (Label label in _gridContainer.GetChildren())
        {
            if (label.Text.StartsWith(itemName))
            {
                var currentQuantity = int.Parse(label.Text.Split('x')[1].Trim());
                var newQuantity = currentQuantity + quantity;
                label.Text = $"{itemName} x{newQuantity}";
                label.TooltipText = $"Quantity: {newQuantity}";
                itemExists = true;
                break;
            }
        }

        if (!itemExists)
        {
            var newItemLabel = new Label
            {
                Text = $"{itemName} x{quantity}",
                TooltipText = $"Quantity: {quantity}",
            };
            _gridContainer.AddChild(newItemLabel);
        }
    }


    public async void UpdateInventoryUI()
    {

        var playerInventory = await _inventoryRepository.GetPlayerInventoryAsync();
        if (playerInventory != null)
        {
            foreach (var item in playerInventory.LootItems)
            {
                var itemLabel = new Label
                {
                    Text = $"{item.Name} x{item.Quantity}",
                    TooltipText = $"Type: {item.Type}\nQuantity: {item.Quantity}",
                };

                _gridContainer.AddChild(itemLabel);
            }
        }
        else
        {
            GD.Print("No items found in the inventory.");
        }
    }
}

