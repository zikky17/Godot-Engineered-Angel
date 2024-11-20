using Godot;
using System.Collections.Generic;
using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Database.Models;

public partial class InventoryUi : Control
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;

    public override void _Ready()
    {
        var lootNodes = GetTree().GetNodesInGroup("Loot");

        foreach (Node lootNode in lootNodes)
        {
            if (lootNode is Loot loot)
            {
                loot.Connect(nameof(Loot.ItemPickedUpEventHandler), new Callable(this, nameof(OnItemPickedUp)));
            }
        }

        _gridContainer = GetNode<GridContainer>("PanelContainer/ScrollContainer/GridContainer");
        _inventoryRepository = new PlayerInventoryRepository();

        UpdateInventoryUI();
    }

    private void OnItemPickedUp(string itemName, string itemType, int quantity)
    {
        GD.Print($"Received Signal: {quantity} x {itemName} ({itemType}) picked up.");
        UpdateInventoryUI();
    }

    public async void UpdateInventoryUI()
    {
        var playerInventory = await _inventoryRepository.GetPlayerInventoryAsync();
        if (playerInventory != null)
        {
            foreach (var item in playerInventory.LootItems)
            {
                bool itemExists = false;
                foreach (Label label in _gridContainer.GetChildren())
                {
                    if (label.Text.StartsWith(item.Name))
                    {
                        var existingQuantity = int.Parse(label.Text.Split('x')[1].Trim());
                        var newQuantity = existingQuantity + item.Quantity;
                        label.Text = $"{item.Name} x{newQuantity}";
                        label.TooltipText = $"Type: {item.Type}\nQuantity: {newQuantity}";
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists)
                {
                    var itemLabel = new Label
                    {
                        Text = $"{item.Name} x{item.Quantity}",
                        TooltipText = $"Type: {item.Type}\nQuantity: {item.Quantity}",
                    };

                    _gridContainer.AddChild(itemLabel);
                }
            }
        }
        else
        {
            GD.Print("No items found in the inventory.");
        }
    }
}
