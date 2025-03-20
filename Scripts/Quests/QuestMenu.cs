using EngineeredAngel.Models.QuestModels;
using EngineeredAngel.Services;
using Godot;
using System;
using System.Collections.Generic;

public partial class QuestMenu : TextureRect
{
    private Label _questNameLabel;
    private Label _questDescriptionLabel;
    private QuestService _questService = new();

    public override void _Ready()
    {
        _questNameLabel = GetNode<Label>("Quests/QuestName");
        _questDescriptionLabel = GetNode<Label>("Quests/QuestName/QuestText");

        Dictionary<string, QuestData> quests = _questService.LoadAllQuests();
        if (quests != null && quests.Count > 0)
        {
            foreach (var quest in quests)
            {
                QuestData questData = quest.Value;

                _questNameLabel.Text = questData.Name;
                _questDescriptionLabel.Text = $"{questData.Description} {questData.KillCount}";
                GD.Print($"{questData.Monster}");
            }
        }
        else
        {
            _questNameLabel.Text = "No active quests";
            _questDescriptionLabel.Text = "";
        }



        if (GameManager.Instance != null)
        {
            GameManager.Instance.Connect(nameof(GameManager.QuestAcceptedEventHandler), new Callable(this, nameof(OnQuestAccepted)));
        }
    }

    private void OnQuestAccepted(string questName, string questText, int monstersToKill, string monster, string npc, bool isCompleted)
    {
        _questService.SaveQuest(questName, questText, monstersToKill, monster, npc, isCompleted);
        UpdateQuestsUI();
    }

    public void UpdateQuestsUI()
    {
        _questNameLabel = GetNode<Label>("Quests/QuestName");
        _questDescriptionLabel = GetNode<Label>("Quests/QuestName/QuestText");

        if (_questNameLabel == null || _questDescriptionLabel == null)
        {
            GD.PrintErr("QuestMenu: Labels are not initialized!");
            return;
        }

        Dictionary<string, QuestData> quests = _questService.LoadAllQuests();
        if (quests != null && quests.Count > 0)
        {
            foreach (var quest in quests)
            {
                QuestData questData = quest.Value;

                _questNameLabel.Text = questData.Name;
                _questDescriptionLabel.Text = $"{questData.Description} {questData.KillCount}";
                GD.Print($"{questData.Monster}");
            }
        }
        else
        {
            _questNameLabel.Text = "No active quests";
            _questDescriptionLabel.Text = "";
        }
    }

}
