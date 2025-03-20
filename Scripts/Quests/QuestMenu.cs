using EngineeredAngel.Models.QuestModels;
using EngineeredAngel.Services;
using Godot;
using System;
using System.Collections.Generic;

public partial class QuestMenu : TextureRect
{
    private Label _questNameLabel;
    private Label _rewardTextLabel;
    private Label _questDescriptionLabel;
    private Label _questRewardGoldLabel;
    private Label _questRewardExperienceLabel;
    private Label _questRewardItemLabel;
    private QuestService _questService = new();

    public override void _Ready()
    {
        _questNameLabel = GetNode<Label>("Quests/QuestPanel/QuestName");
        _questDescriptionLabel = GetNode<Label>("Quests/QuestPanel/QuestText");
        _rewardTextLabel = GetNode<Label>("Quests/QuestPanel/RewardText");
        _questRewardGoldLabel = GetNode<Label>("Quests/QuestPanel/RewardTextGold");
        _questRewardExperienceLabel = GetNode<Label>("Quests/QuestPanel/RewardTextExperience");
        _questRewardItemLabel = GetNode<Label>("Quests/QuestPanel/RewardTextItem");

        Dictionary<string, QuestData> quests = _questService.LoadAllQuests();
        if (quests != null && quests.Count > 0)
        {
            foreach (var quest in quests)
            {
                QuestData questData = quest.Value;

                _questNameLabel.Text = questData.Name;
                _questDescriptionLabel.Text = $"{questData.Description} {questData.KillCount}";
                _rewardTextLabel.Text = "Rewards:";
                _questRewardGoldLabel.Text = $"{questData.QuestReward.Gold} Gold";
                _questRewardExperienceLabel.Text = $"{questData.QuestReward.Experience} Experience";
                _questRewardItemLabel.Text = $"1x {questData.QuestReward.ItemReward}";
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

    private void OnQuestAccepted(
        string questName, 
        string questText, 
        int monstersToKill, 
        string monster, 
        string npc, 
        bool isCompleted,
        int goldReward,
        int experienceReward,
        string itemReward)
    {
        _questService.SaveQuest(questName, questText, monstersToKill, monster, npc, isCompleted, goldReward, experienceReward, itemReward);
        UpdateQuestsUI();
    }

    public void UpdateQuestsUI()
    {
        _questNameLabel = GetNode<Label>("Quests/QuestPanel/QuestName");
        _questDescriptionLabel = GetNode<Label>("Quests/QuestPanel/QuestText");

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
