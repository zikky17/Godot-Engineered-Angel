using Godot;
using EngineeredAngel.Database.DbServices;

public partial class InventoryUi : PanelContainer
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;

    public override void _Ready()
    {
        var viewportSize = GetViewportRect().Size;
        SetSize(new Vector2(viewportSize.X * 0.3f, viewportSize.Y * 0.5f));
        SetPosition(new Vector2(viewportSize.X - viewportSize.X * 0.35f, 25));

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

        _gridContainer = GetNode<GridContainer>("GridContainer");
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
        foreach (InventorySlot slot in _gridContainer.GetChildren())
        {
            if (slot.Name == itemName)
            {
                var currentQuantity = int.Parse(slot.GetNode<Label>("ItemCount").Text.Replace("x", ""));
                var newQuantity = currentQuantity + quantity;
                slot.UpdateSlot(slot.GetNode<TextureRect>("ItemPicture").Texture, newQuantity);
                GD.Print($"Updated {itemName} to new quantity: {newQuantity}");
                return;
            }
        }

        foreach (InventorySlot slot in _gridContainer.GetChildren())
        {
            if (slot.GetNode<Label>("ItemCount").Text == "x0")
            {
                var texture = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/{itemName.Replace(" ", "")}.png");
                if (texture == null)
                {
                    GD.Print($"Failed to load texture for {itemName}");
                }
                else
                {
                    GD.Print($"Successfully loaded texture: {texture.ResourcePath}");
                }
                slot.Name = itemName;
                slot.UpdateSlot(texture, quantity);
                GD.Print($"Added new item: {itemName} with quantity: {quantity}");
                return;
            }
        }

        GD.Print($"No empty slots available for {itemName}");
    }

    public async void UpdateInventoryUI()
    {
        var playerInventory = await _inventoryRepository.GetPlayerInventoryAsync();
        if (playerInventory != null)
        {
            foreach (var item in playerInventory.LootItems)
            {
                foreach (InventorySlot slot in _gridContainer.GetChildren())
                {
                    if (slot.GetNode<Label>("ItemCount").Text == "x0")
                    {
                        var texture = GD.Load<Texture2D>("res://Assets/Sprites/Loot/IronSword.png");
                        slot.Name = item.Name;
                        slot.UpdateSlot(texture, item.Quantity);
                        break;
                    }
                }
            }
        }
        else
        {
            GD.Print("No items found in the inventory.");
        }
    }
}
