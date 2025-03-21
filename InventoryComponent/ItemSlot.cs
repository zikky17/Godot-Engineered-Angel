using Godot;
using EngineeredAngel.Loot;

public partial class ItemSlot : VBoxContainer
{
    public LootItem Loot { get; private set; }
    public InventoryUi ParentInventory { get; set; }

    public void SetLoot(LootItem loot)
    {
        Loot = loot;
        this.Name = loot.Name;

        var itemPicture = GetNode<TextureRect>("ItemPicture");
        var itemCount = GetNode<Label>("ItemCount");

        itemPicture.Texture = LoadTexture(loot);
        itemCount.Text = $"x{loot.Quantity}";

        itemPicture.TooltipText = $"Name: {loot.Name}\nType: {loot.Type}\nTier: {loot.Tier}\n" +
                                  $"Rarity: {loot.Rarity}\n" +
                                  $"Attack: {loot.Attack}\nDefense: {loot.Defense}\n" +
                                  $"Special Effect: {loot.SpecialEffect}\nAmplified Damage: {loot.AmplifiedDamage}";

        if (!itemPicture.IsConnected("gui_input", new Callable(this, nameof(OnRightClick))))
            itemPicture.Connect("gui_input", new Callable(this, nameof(OnRightClick)));
    }

    private Texture2D LoadTexture(LootItem loot)
    {
        string path = loot.Rarity switch
        {
            "Common" => $"res://Assets/Sprites/Loot/{loot.Name.Replace(" ", "")}.png",
            "Rare" => $"res://Assets/Sprites/Loot/RareItems/{loot.Name.Replace(" ", "")}_Rare.png",
            "Epic" => $"res://Assets/Sprites/Loot/EpicItems/{loot.Name.Replace(" ", "")}_Epic.png",
            _ => null
        };

        return !string.IsNullOrEmpty(path) ? GD.Load<Texture2D>(path) : null;
    }

    private void OnRightClick(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
        {
            if (Loot != null && ParentInventory != null)
            {
                ParentInventory.ShowOptionsForItem(this);
            }
        }
    }

    public void ClearSlot()
    {
        GetNode<TextureRect>("ItemPicture").Texture = null;
        GetNode<Label>("ItemCount").Text = "x0";
        this.Name = "Empty";
        Loot = null;
    }
}
