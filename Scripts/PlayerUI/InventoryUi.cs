using Godot;
using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Loot;
using EngineeredAngel.Factories;
using EngineeredAngel.Enums;

public partial class InventoryUi : TextureRect
{
    private GridContainer _gridContainer;
    private PlayerInventoryRepository _inventoryRepository;
    private Zikky _zikky;
    private PopupMenu _optionsMenu;


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

        _optionsMenu = GetNode<PopupMenu>("OptionsMenu");
        _gridContainer = GetNode<GridContainer>("PanelContainer/GridContainer");
        _zikky = GetNode<Zikky>("../../../Zikky");
        _inventoryRepository = new PlayerInventoryRepository();

        UpdateInventoryUI();
    }

    private void OnItemPickedUp(
        string name,
        string type,
        int quantity,
        string rarity,
        int attack,
        int defense,
        int tier,
        string specialEffect,
        int amplifiedDamage
    )
    {
        GD.Print($"Received Signal: {quantity} x {name} ({type}) picked up.");

        var loot = new LootItem
        {
            Name = name,
            Type = type,
            Quantity = quantity,
            Rarity = rarity,
            Attack = attack,
            Defense = defense,
            Tier = tier,
            SpecialEffect = specialEffect,
            AmplifiedDamage = amplifiedDamage
        };

        AddNewItemToUI(loot);
    }

    private void AddNewItemToUI(LootItem loot)
    {
        GD.Print($"Adding new item: {loot.Name} to inventory...");

        if (loot.Type == "Potion" || loot.Type == "OtherStackableType")
        {
            foreach (VBoxContainer slot in _gridContainer.GetChildren())
            {
                var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
                var itemCount = slot.GetNode<Label>("ItemCount");

                if (slot.Name == loot.Name)
                {
                    int currentQuantity = int.Parse(itemCount.Text.Substring(1));
                    currentQuantity += loot.Quantity;
                    itemCount.Text = $"x{currentQuantity}";
                    GD.Print($"Updated quantity for {loot.Name} to {currentQuantity}.");
                    return; 
                }
            }
        }

        foreach (VBoxContainer slot in _gridContainer.GetChildren())
        {
            var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
            var itemCount = slot.GetNode<Label>("ItemCount");

            if (itemCount.Text == "x0")
            {
                switch (loot.Rarity)
                {
                    case "Common":
                        var texture = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/{loot.Name.Replace(" ", "")}.png");
                        itemPicture.Texture = texture;
                        break;
                    case "Rare":
                        var textureRare = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/RareItems/{loot.Name.Replace(" ", "")}_Rare.png");
                        itemPicture.Texture = textureRare;
                        break;
                    case "Epic":
                        var textureEpic = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/EpicItems/{loot.Name.Replace(" ", "")}_Epic.png");
                        itemPicture.Texture = textureEpic;
                        break;
                }

                slot.Name = loot.Name;
                itemCount.Text = $"{loot.Quantity}";

                itemPicture.TooltipText = $"Name: {loot.Name}\nType: {loot.Type}\nTier: {loot.Tier}\n" +
                                          $"Rarity: {loot.Rarity}\n" +
                                          $"Attack: {loot.Attack}\nDefense: {loot.Defense}\n" +
                                          $"Special Effect: {loot.SpecialEffect}\nAmplified Damage: {loot.AmplifiedDamage}";

                GD.Print($"Added new item: {loot.Name} to inventory.");
                return;
            }
        }

        _zikky.HasInventorySpace(false);
    }


    public async void UpdateInventoryUI()
    {
        var playerInventory = await _inventoryRepository.GetPlayerInventoryAsync();
        if (playerInventory != null)
        {
            foreach (var item in playerInventory.LootItems)
            {
                var lootItem = LootFactory.CreateLootItem(item);
                AddNewItemToUI(lootItem);
            }
        }
        else
        {
            GD.Print("No items found in the inventory.");
        }
    }


    public void VoidTouch(LootItem loot, VBoxContainer slot)
    {
        int scrapsGained = loot.Quantity * loot.Tier; 
        _zikky.CharacterStats.VoidScraps += scrapsGained;

        GD.Print($"Recycled {loot.Name}, gained {scrapsGained} Iron Scraps. Total: {_zikky.CharacterStats.VoidScraps}");

        var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
        var itemCount = slot.GetNode<Label>("ItemCount");

        itemPicture.Texture = null;
        itemCount.Text = "x0";
        slot.Name = "Empty";

        RemoveFromInventoryDatabase(loot);

    }

    private void RemoveFromInventoryDatabase(LootItem loot)
    {
        _inventoryRepository.RemoveLootFromDatabase(loot);
        GD.Print($"Removed {loot.Name} from database.");
    }
}
