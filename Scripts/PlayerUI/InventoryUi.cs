using Godot;
using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Loot;
using EngineeredAngel.Factories;

public partial class InventoryUi : TextureRect
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

        _gridContainer = GetNode<GridContainer>("PanelContainer/GridContainer");
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

        foreach (VBoxContainer slot in _gridContainer.GetChildren())
        {
            var itemPicture = slot.GetNode<TextureRect>("ItemPicture");
            var itemCount = slot.GetNode<Label>("ItemCount");

            if (itemCount.Text == "x0")
            {
                var texture = GD.Load<Texture2D>($"res://Assets/Sprites/Loot/{loot.Name.Replace(" ", "")}.png");
                if (texture == null)
                {
                    GD.PrintErr($"Failed to load texture for {loot.Name}");
                    return;
                }

                if (loot.Rarity == "Rare")
                {
                    var background = new ColorRect
                    {
                        Color = new Color(0.2f, 0.4f, 1.0f, 1.0f),
                        AnchorLeft = 0,
                        AnchorTop = 0,
                        AnchorRight = 1,
                        AnchorBottom = 1
                    };

                    slot.AddChild(background);
                    background.MoveChild(itemPicture, 1);
                }

                slot.Name = loot.Name;
                itemPicture.Texture = texture;
                itemCount.Visible = true;

                itemPicture.TooltipText = $"Name: {loot.Name}\nType: {loot.Type}\nTier: {loot.Tier}\n" +
                                          $"Rarity: {loot.Rarity}\n" +
                                          $"Attack: {loot.Attack}\nDefense: {loot.Defense}\n" +
                                          $"Special Effect: {loot.SpecialEffect}\nAmplified Damage: {loot.AmplifiedDamage}";

                itemCount.Text = $"x{loot.Quantity}";

                GD.Print($"Added new item: {loot.Name} to inventory.");

                if (!itemPicture.IsConnected("mouse_entered", Callable.From(() => { })))
                {
                    itemPicture.MouseFilter = Control.MouseFilterEnum.Pass;
                    itemPicture.Connect("mouse_entered", Callable.From(() =>
                    {
                        GD.Print($"Mouse entered on {loot.Name}");
                    }));
                }

                return;
            }
        }

        GD.Print($"No empty slots available for {loot.Name}");
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

}
