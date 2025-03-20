using DialogueManagerRuntime;
using Godot;
using System;

public partial class NPC_Iziba : CharacterBody2D
{
    private Area2D _talkZone;
    private bool GotQuest = false;
    private Zikky _zikky;
    private Timer _newQuestLabelTimer;

    public override void _Ready()
    {
        _zikky = GetNode<Zikky>("../Zikky");
        _talkZone = GetNode<Area2D>("TalkZone");
        _talkZone.Connect("body_entered", new Callable(this, nameof(OnTalkZoneBodyEntered)));

        _newQuestLabelTimer = new Timer();
        _newQuestLabelTimer.WaitTime = 5; 
        _newQuestLabelTimer.OneShot = true; 
        _newQuestLabelTimer.Timeout += OnNewQuestLabelTimerTimeout;
        AddChild(_newQuestLabelTimer);
    }

    private void OnTalkZoneBodyEntered(Node body)
    {
        if (body.IsInGroup("Player") && GotQuest == false)
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
            var questLabel = _zikky.GetNode<Label>("NewQuestLabel");
            questLabel.Visible = true;

            _newQuestLabelTimer.Start();
        };
    }

    private void OnNewQuestLabelTimerTimeout()
    {
        var questLabel = _zikky.GetNode<Label>("NewQuestLabel");
        questLabel.Visible = false;
        GameManager.Instance.EmitSignal(
              nameof(GameManager.QuestAcceptedEventHandler),
              "Gorgon Slayer",
              "Gorgons left to kill:",
              10, "Gorgon");
    }
}
