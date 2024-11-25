using Godot;
using EngineeredAngel.Database.DbServices;

public partial class InventoryUi : TextureRect
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;

    public override void _Ready()
    {
        //var viewportSize = GetViewportRect().Size;
        //SetSize(new Vector2(viewportSize.X * 0.3f, viewportSize.Y * 0.5f));
        //SetPosition(new Vector2(viewportSize.X - viewportSize.X * 0.35f, 25));

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

        _gridContainer = GetNode<GridContainer>("PanelContainer/GridContainer");
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

        foreach (VBoxContainer slot in _gridContainer.GetChildren())
        {
            var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
            var itemCount = slot.GetNode<Label>("ItemCount");

            if (slot.Name == itemName)
            {
                var currentQuantity = int.Parse(itemCount.Text.Replace("x", ""));
                var newQuantity = currentQuantity + quantity;
                itemPicture.Texture = itemPicture.Texture;
                itemCount.Text = $"x{newQuantity}";
                GD.Print($"Updated {itemName} to new quantity: {newQuantity}");
                return;
            }

            if (itemCount.Text == "x0") 
            {
                var texture = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/{itemName.Replace(" ", "")}.png");
                if (texture == null)
                {
                    GD.PrintErr($"Failed to load texture for {itemName}");
                    return;
                }
                slot.Name = itemName;
                itemPicture.Texture = texture;
                itemCount.Text = $"x{quantity}";
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
                foreach (VBoxContainer slot in _gridContainer.GetChildren())
                {
                    var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
                    var itemCount = slot.GetNode<Label>("ItemCount");

                    if (itemCount.Text == "x0")
                    {
                        var texture = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/{item.Name.Replace(" ", "")}.png");
                        if (texture == null)
                        {
                            GD.PrintErr($"Failed to load texture for {item.Name}");
                            return;
                        }
                        slot.Name = item.Name;
                        itemPicture.Texture = texture;
                        itemCount.Text = $"x{item.Quantity}";
                        GD.Print($"Successfully loaded texture: {texture.ResourceName}");
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
