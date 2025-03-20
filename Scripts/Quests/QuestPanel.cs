using Godot;
using System;

public partial class QuestPanel : Panel
{
    private bool _isExpanded = false;
    private Node _questDetails;

    public override void _Ready()
    {
        _questDetails = GetParent();
        ToggleQuestDetails(false);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            _isExpanded = !_isExpanded;
            ToggleQuestDetails(_isExpanded);
            GD.Print("Quest Name pressed");
        }
    }

    private void ToggleQuestDetails(bool show)
    {
        foreach (Node child in GetChildren())
        {
            if (child is Label labelNode && labelNode.Name != "QuestName")
            {
                labelNode.Visible = show;
            }
        }

        var label = GetNode<Label>("QuestName");
        label.Text = show ? "▼ " + label.Text.TrimStart('▶', '▼', ' ') : "▶ " + label.Text.TrimStart('▶', '▼', ' ');
    }
}
