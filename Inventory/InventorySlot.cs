using Godot;

public partial class InventorySlot : VBoxContainer
{
    private TextureRect _icon;
    private Label _quantityLabel;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("ItemPicture");
        _quantityLabel = GetNode<Label>("ItemCount");
    }

    public void UpdateSlot(Texture2D texture, int quantity)
    {
        GD.Print($"UpdateSlot called with texture: {texture?.ResourcePath}, quantity: {quantity}");
        _icon.Texture = texture;
        _quantityLabel.Text = $"x{quantity}";
        _quantityLabel.TooltipText = $"Quantity: {quantity}";
    }

}
