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
    private TextureRect _itemPicture;
    private LootItem _selectedLoot;
    private VBoxContainer _selectedSlot;
    private TextureRect _equippedWeapon;


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
        _optionsMenu.AddItem("Equip Item", 1);
        _optionsMenu.AddItem("Destroy Item", 0);
        _optionsMenu.IdPressed += OnOptionsMenuItemSelected;
        _gridContainer = GetNode<GridContainer>("PanelContainer/GridContainer");
        _zikky = GetNode<Zikky>("../../../Zikky");
        _inventoryRepository = new PlayerInventoryRepository();
        var equippedWeapon = _inventoryRepository.LoadEquippedWeapon();
        ShowEquippedWeapon(equippedWeapon);
        UpdateInventoryUI();
    }

    private void OnItemRightClick(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
        {
            foreach (ItemSlot slot in _gridContainer.GetChildren())
            {
                var itemPic = slot.GetNode<TextureRect>("ItemPicture");
                if (itemPic == GetViewport().GuiGetFocusOwner())
                {
                    _selectedSlot = slot;
                    _selectedLoot = slot.Loot;
                    break;
                }
            }

            if (_selectedLoot != null)
            {
                Vector2 mousePos = GetViewport().GetMousePosition();
                _optionsMenu.Position = new Vector2I((int)mousePos.X, (int)mousePos.Y);
                _optionsMenu.Popup();
            }
        }
    }

    private void OnOptionsMenuItemSelected(long id)
    {
        if (_selectedLoot != null && _selectedSlot is ItemSlot itemSlot)
        {
            switch (id)
            {
                case 0:
                    VoidTouch(_selectedLoot, itemSlot);
                    break;
                case 1:
                    EquipItem(_selectedLoot, itemSlot);
                    break;
            }
        }

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

        foreach (ItemSlot slot in _gridContainer.GetChildren())
        {
            if (slot.Loot == null)
            {
                var itemPicture = slot.GetNode<TextureRect>("ItemPicture");

                string path = loot.Rarity switch
                {
                    "Common" => $"res://Assets/Sprites/Loot/{loot.Name.Replace(" ", "")}.png",
                    "Rare" => $"res://Assets/Sprites/Loot/RareItems/{loot.Name.Replace(" ", "")}_Rare.png",
                    "Epic" => $"res://Assets/Sprites/Loot/EpicItems/{loot.Name.Replace(" ", "")}_Epic.png",
                    _ => null
                };

                if (!string.IsNullOrEmpty(path))
                    itemPicture.Texture = GD.Load<Texture2D>(path);
                slot.ParentInventory = this;
                slot.SetLoot(loot);
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

    public void ShowOptionsForItem(ItemSlot slot)
    {
        _selectedSlot = slot;
        _selectedLoot = slot.Loot;

        Vector2 mousePos = GetViewport().GetMousePosition();
        _optionsMenu.Position = new Vector2I((int)mousePos.X, (int)mousePos.Y);
        _optionsMenu.Popup();
    }

    public void EquipItem(LootItem loot, ItemSlot slot)
    {
        var weaponEquippedSlot = GetNode<TextureRect>("../PlayerUI/WeaponContainer/WeaponEquipped");
        var itemTexture = slot.GetNode<TextureRect>("ItemPicture").Texture;

        if (itemTexture != null)
        {
            weaponEquippedSlot.Texture = itemTexture;

            weaponEquippedSlot.TooltipText = $"Name: {loot.Name}\n" +
                                             $"Type: {loot.Type}\n" +
                                             $"Tier: {loot.Tier}\n" +
                                             $"Rarity: {loot.Rarity}\n" +
                                             $"Attack: {loot.Attack}\n" +
                                             $"Defense: {loot.Defense}\n" +
                                             $"Special Effect: {loot.SpecialEffect}\n" +
                                             $"Amplified Damage: {loot.AmplifiedDamage}";
        }

        slot.ClearSlot();
        RemoveFromInventoryDatabase(loot);
        
        GD.Print($"Equipped weapon: {loot.Name}");
    }

    private void ShowEquippedWeapon(LootItem loot)
    {
        var weaponEquippedSlot = GetNode<TextureRect>("../PlayerUI/WeaponContainer/WeaponEquipped");

        if (loot == null)
        {
            weaponEquippedSlot.Texture = GD.Load<Texture2D>("res://Assets/UI/Icons/placeholder_sword.png");
            weaponEquippedSlot.TooltipText = "No weapon equipped.";
            return;
        }

        string path = loot.Rarity switch
        {
            "Common" => $"res://Assets/Sprites/Loot/{loot.Name.Replace(" ", "")}.png",
            "Rare" => $"res://Assets/Sprites/Loot/RareItems/{loot.Name.Replace(" ", "")}_Rare.png",
            "Epic" => $"res://Assets/Sprites/Loot/EpicItems/{loot.Name.Replace(" ", "")}_Epic.png",
            _ => null
        };

        if (!string.IsNullOrEmpty(path))
            weaponEquippedSlot.Texture = GD.Load<Texture2D>(path);

        weaponEquippedSlot.TooltipText = $"Name: {loot.Name}\n" +
                                         $"Type: {loot.Type}\n" +
                                         $"Tier: {loot.Tier}\n" +
                                         $"Rarity: {loot.Rarity}\n" +
                                         $"Attack: {loot.Attack}\nDefense: {loot.Defense}\n" +
                                         $"Special Effect: {loot.SpecialEffect}\n" +
                                         $"Amplified Damage: {loot.AmplifiedDamage}";
    }



    public void VoidTouch(LootItem loot, ItemSlot slot)
    {
        int scrapsGained = loot.Quantity * loot.Tier;
        _zikky.CharacterStats.VoidScraps += scrapsGained;

        GD.Print($"Recycled {loot.Name}, gained {scrapsGained} Iron Scraps. Total: {_zikky.CharacterStats.VoidScraps}");

        slot.ClearSlot();
        RemoveFromInventoryDatabase(loot);
    }

    private void RemoveFromInventoryDatabase(LootItem loot)
    {
        _inventoryRepository.RemoveLootFromDatabase(loot);
        GD.Print($"Removed {loot.Name} from database.");
    }

    private void AddItemToEquippedItems(LootItem loot)
    {
        _inventoryRepository.AddEquippedWeapon(loot);
        GD.Print($"Removed {loot.Name} from database.");
    }
}
