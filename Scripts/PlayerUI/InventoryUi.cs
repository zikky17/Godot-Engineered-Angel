using Godot;
using EngineeredAngel.Database.DbServices;

public partial class InventoryUi : Control
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;

    public override void _Ready()
    {
        var viewportSize = GetViewportRect().Size;

        SetSize(new Vector2(viewportSize.X * 0.3f, viewportSize.Y * 0.5f));
        SetPosition(new Vector2(viewportSize.X - viewportSize.Y - 25, 25));

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

        foreach (VBoxContainer itemContainer in _gridContainer.GetChildren())
        {
            if (itemContainer.Name == itemName)
            {
                var quantityLabel = itemContainer.GetNode<Label>("QuantityLabel");
                var currentQuantity = int.Parse(quantityLabel.Text.Trim().Split('x')[1]);
                var newQuantity = currentQuantity + quantity;
                quantityLabel.Text = $"x{newQuantity}";
                quantityLabel.TooltipText = $"Quantity: {newQuantity}";

                GD.Print($"Updated {itemName} to new quantity: {newQuantity}");
                itemExists = true;
                break;
            }
        }

        if (!itemExists)
        {
            var itemContainer = new VBoxContainer
            {
                Name = itemName
            };

            var newItemIcon = new TextureRect
            {
                Texture = (Texture2D)GD.Load<Texture>($"res://Assets/Sprites/Loot/{itemName.Replace(" ", "")}.png"),
                TooltipText = $"Quantity: {quantity}"
            };
            itemContainer.AddChild(newItemIcon);

            var quantityLabel = new Label
            {
                Name = "QuantityLabel",
                Text = $"x{quantity}",
                TooltipText = $"Quantity: {quantity}"
            };
            itemContainer.AddChild(quantityLabel);

            _gridContainer.AddChild(itemContainer);

            GD.Print($"Added new item: {itemName} with quantity: {quantity}");
        }
    }

    public async void UpdateInventoryUI()
    {
        ClearGridContainer();

        var playerInventory = await _inventoryRepository.GetPlayerInventoryAsync();
        if (playerInventory != null)
        {
            foreach (var item in playerInventory.LootItems)
            {
                var itemContainer = new VBoxContainer
                {
                    Name = item.Name
                };

                var itemIcon = new TextureRect
                {
                    Texture = (Texture2D)GD.Load<Texture>($"res://Assets/Sprites/Loot/{item.Name.Replace(" ", "")}.png"),
                    TooltipText = $"Quantity: {item.Quantity}"
                };
                itemContainer.AddChild(itemIcon);

                var quantityLabel = new Label
                {
                    Name = "QuantityLabel",
                    Text = $"x{item.Quantity}",
                    TooltipText = $"Quantity: {item.Quantity}"
                };
                itemContainer.AddChild(quantityLabel);

                _gridContainer.AddChild(itemContainer);
            }
        }
        else
        {
            GD.Print("No items found in the inventory.");
        }
    }

    private void ClearGridContainer()
    {
        foreach (Node child in _gridContainer.GetChildren())
        {
            child.QueueFree();
        }
    }
}
