using DialogueManagerRuntime;
using EngineeredAngel.Models.QuestModels;
using EngineeredAngel.Services;
using Godot;
using System;

public partial class NPC_Iziba : CharacterBody2D
{
    private Area2D _talkZone;
    private bool GotQuest = false;
    private bool QuestCompleted = false;
    private Player _player;
    private Timer _newQuestLabelTimer;
    private Timer _completedQuestLabelTimer;
    private QuestService _questService = new();
    private QuestMenu _questMenu;
    public readonly LevelUpService _levelUpService;

    public override void _Ready()
    {
        _player = GetNode<Player>("../Player");
        _talkZone = GetNode<Area2D>("TalkZone");
        _talkZone.Connect("body_entered", new Callable(this, nameof(OnTalkZoneBodyEntered)));
        _questMenu = GetNodeOrNull<QuestMenu>("../Player/CharacterMenus/QuestMenu");

        _newQuestLabelTimer = new Timer();
        _newQuestLabelTimer.WaitTime = 5;
        _newQuestLabelTimer.OneShot = true;
        _newQuestLabelTimer.Timeout += OnNewQuestLabelTimerTimeout;
        AddChild(_newQuestLabelTimer);

        _completedQuestLabelTimer = new Timer();
        _completedQuestLabelTimer.WaitTime = 7;
        _completedQuestLabelTimer.OneShot = true;
        _completedQuestLabelTimer.Timeout += OnCompletedQuestLabelTimerTimeout;
        AddChild(_completedQuestLabelTimer);
    }

    private void OnTalkZoneBodyEntered(Node body)
    {
        var activeQuests = _questService.LoadAllQuests();
        if (activeQuests != null)
        {
            foreach (var quest in activeQuests)
            {
                if (quest.Value.IsCompleted && quest.Value.NPC == "Iziba")
                {
                    QuestCompleted = true;
                    var dialogueResource = (Resource)GD.Load("res://dialogue/Iziba/iziba_completed_quest.dialogue");
                    CallDeferred(nameof(ShowQuestCompletedDialogue), dialogueResource);
                    _questService.SaveQuest("Gorgon Slayer (Completed)", null, null, "", "", true, 0, 0, null);
                    _questMenu.UpdateQuestsUI();
                }
            }
        }

        if (QuestCompleted)
        {
            return;
        }

        if (body.IsInGroup("Player") && GotQuest == false && QuestCompleted == false)
        {
            var dialogueResource = (Resource)GD.Load("res://dialogue/Iziba/iziba_1.dialogue");
            CallDeferred(nameof(ShowDialogue), dialogueResource);
        }
    }


    private void ShowDialogue(Resource dialogueResource)
    {
        DialogueManager.ShowExampleDialogueBalloon(dialogueResource, "iziba_dialogue");
        GotQuest = true;

        DialogueManager.DialogueEnded += (Resource dialogueResource) =>
        {
            var questLabel = _player.GetNode<Label>("NewQuestLabel");
            questLabel.Visible = true;

            _newQuestLabelTimer.Start();
        };
    }

    private void ShowQuestCompletedDialogue(Resource dialogueResource)
    {
        DialogueManager.ShowExampleDialogueBalloon(dialogueResource, "iziba_completed_quest");

        DialogueManager.DialogueEnded += (Resource dialogueResource) =>
        {
            var questLabel = _player.GetNode<Label>("QuestCompletedLabel");
            questLabel.Visible = true;

            _completedQuestLabelTimer.Start();
        };
    }

    private void OnNewQuestLabelTimerTimeout()
    {
        var questLabel = _player.GetNode<Label>("NewQuestLabel");
        var questRewards = new QuestReward
        {
            Gold = 250,
            Experience = 100,
            ItemReward = "Steel Armor"
        };
        questLabel.Visible = false;
        GameManager.Instance.EmitSignal(
              nameof(GameManager.QuestAcceptedEventHandler),
              "Gorgon Slayer",
              "Gorgons left to kill:",
              10, "Gorgon", "Iziba", false, questRewards.Gold, questRewards.Experience, questRewards.ItemReward);
    }

    private void OnCompletedQuestLabelTimerTimeout()
    {
        var questLabel = _player.GetNode<Label>("QuestCompletedLabel");
        questLabel.Visible = false;
    }
}
