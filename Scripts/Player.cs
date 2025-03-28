using DialogueManagerRuntime;
using EngineeredAngel.Database.DbServices;
using EngineeredAngel.Database.Models;
using EngineeredAngel.Interfaces;
using EngineeredAngel.Models.Player;
using EngineeredAngel.PlayerStates;
using EngineeredAngel.Services;
using EngineeredAngel.Stats;
using Godot;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{

    public PlayerStats CharacterStats { get; set; }
    public RewardService RewardService { get; set; }

    [Export] public int Speed = 200;
    public Vector2 LastDirection { get; set; } = Vector2.Zero;
    public AnimatedSprite2D AnimatedSprite { get; private set; }
    public AnimatedSprite2D HealingAnimation { get; private set; }
    public bool RecentlyHealed = false;
    public AudioStream attackSound;
    public AudioStreamPlayer _audioPlayer;
    private IPlayerState _currentState;
    private Area2D _hitbox;
    public ProgressBar Health;
    public bool IsDead = false;
    public bool IsAttacking = false;
    public bool HasTakenDamage = false;
    public bool EnemyInRange { get; set; } = false;
    private bool hasPlayedIntro = false;

    private Label _damageLabel;
    private Timer _combatTextTimer;
    private float lastDamageTime = -2f;
    private const float damageCooldown = 2f;

    public bool InventoryFull = false;

    private Label _healingLabel;

    private readonly PlayerDataRepository _playerDataRepository = new PlayerDataRepository();

    public async override void _Ready()
    {
        CharacterStats = await LoadPlayerStats();

        var levelUpService = new LevelUpService();
        RewardService = new RewardService(this, levelUpService);

        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HealingAnimation = GetNode<AnimatedSprite2D>("HealingAnimation");
        _hitbox = GetNode<Area2D>("HitBox");
        _hitbox.Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnHitboxBodyEntered)));
        _hitbox.Connect(Area2D.SignalName.BodyExited, new Callable(this, nameof(OnHitboxBodyExited)));
        HealingAnimation.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(OnHealingAnimationFinished));

        Health = GetNode<ProgressBar>("healthbar");
        Health.Value = CharacterStats.HP;

        _audioPlayer = GetNode<AudioStreamPlayer>("PlayerSounds");
        attackSound = (AudioStream)GD.Load("res://Assets/SoundEffects/ScarySounds/Player_Swing.mp3");

        _damageLabel = GetNode<Label>("DamageLabel");
        _damageLabel.Visible = false;
        _combatTextTimer = new Timer();
        _combatTextTimer.WaitTime = 0.5f;
        _combatTextTimer.OneShot = true;
        AddChild(_combatTextTimer);

        _healingLabel = GetNode<Label>("HealingLabel");
        _healingLabel.Visible = false;
        _combatTextTimer = new Timer();
        _combatTextTimer.WaitTime = 1.0f;
        _combatTextTimer.OneShot = true;
        AddChild(_combatTextTimer);

        _combatTextTimer.Connect("timeout", new Callable(this, nameof(HideLabel)));

        AddToGroup("Player");
        SetState(new IdleState());
    }

    public async Task LoadPlayerProfileOnCharacterCreation(PlayerProfileModel playerProfile)
    {
        var newPlayerData = new GamePlayerEntity
        {
            Level = 1,
            PlayerName = playerProfile.PlayerName,
            ClassName = playerProfile.PlayerClass.ClassName,
            CurrentHP = playerProfile.CurrentHp,
            MaxHealth = playerProfile.MaxHealth,
            Strength = playerProfile.Strength,
            Defence = playerProfile.Defence,
            Gold = 0,
            Experience = 0,
            Intelligence = playerProfile.Intelligence,
            Agility = playerProfile.Agility
        };

        await _playerDataRepository.SavePlayerDataAsync(newPlayerData);

        CharacterStats = new PlayerStats(
            newPlayerData.Level,
            newPlayerData.PlayerName,
            newPlayerData.ClassName,
            newPlayerData.MaxHealth,
            newPlayerData.MaxHealth,
            newPlayerData.Strength,
            newPlayerData.Defence,
            newPlayerData.Gold,
            newPlayerData.Experience,
            newPlayerData.Intelligence,
            newPlayerData.Agility
            );

        Health.Value = CharacterStats.HP;

    }

    private async Task<PlayerStats> LoadPlayerStats()
    {
        var playerData = await _playerDataRepository.GetPlayerDataAsync(1);

        if (playerData == null)
        {
            throw new System.Exception("Player data missing");
        }

        return new PlayerStats(
            playerData.Level,
            playerData.PlayerName,
            playerData.ClassName,
            playerData.MaxHealth,
            playerData.MaxHealth,
            playerData.Strength,
            playerData.Defence,
            playerData.Gold,
            playerData.Experience,
            playerData.Intelligence,
            playerData.Agility
            );
    }


    public override void _PhysicsProcess(double delta)
    {

        if (CharacterStats == null)
        {
            GD.PrintErr("CharacterStats is null, skipping physics processing.");
            return;
        }

        if (CharacterStats.HP <= 0 && !IsDead)
        {
            Die();
        }

        if (Input.IsActionJustPressed("ctrl_action"))
        {
            IsAttacking = true;
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        _currentState?.HandleInput(this, direction, IsAttacking);
        _currentState?.Update(this);
        UpdateHealthBar();


        Velocity = direction * Speed;
        MoveAndSlide();
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (!hasPlayedIntro && Input.IsActionJustPressed("ui_accept"))
        {
            var dialogueResource = (Resource)GD.Load("res://dialogue/intro_dialogue.dialogue");
            DialogueManager.ShowExampleDialogueBalloon(dialogueResource, "start");
            hasPlayedIntro = true;
        }
    }



    public void Heal(int healAmount)
    {
        ShowCombatText(null, healAmount);
        Health.Value += healAmount;
        CharacterStats.HP += healAmount;
        RecentlyHealed = true;
        HealingAnimation.Visible = true;
        HealingAnimation.Play("heal");
        GD.Print($"Zikky healed {healAmount} HP. New HP is now {Health.Value}");
    }

    public void SetState(IPlayerState newState)
    {
        _currentState = newState;
    }

    private void OnHealingAnimationFinished()
    {
        if (RecentlyHealed)
        {
            HealingAnimation.Visible = false;
            RecentlyHealed = false;
        }
    }

    private void OnHitboxBodyEntered(Node body)
    {
        if (body.IsInGroup("Enemy"))
        {
            EnemyInRange = true;
        }
    }

    private void OnHitboxBodyExited(Node body)
    {
        if (body.IsInGroup("Enemy"))
        {
            EnemyInRange = false;
        }
    }

    public void UpdateHealthBar()
    {
        Health.Visible = Health.Value < CharacterStats.MaxHP;
    }

    public void Die()
    {
        IsDead = true;
        AnimatedSprite.Play("die");
        Respawn();
    }

    public void ShowCombatText(int? damage, int? healing)
    {
        if (damage.HasValue)
        {
            _damageLabel.Text = "-" + damage.ToString();
            _damageLabel.Visible = true;
            _combatTextTimer.Start();
        }
        else
        {
            _healingLabel.Text = "+" + healing.ToString();
            _healingLabel.Visible = true;
            _combatTextTimer.Start();
        }

    }

    private void HideLabel()
    {
        _damageLabel.Visible = false;
        _healingLabel.Visible = false;
    }


    private void Respawn()
    {
        IsDead = false;
        CharacterStats.HP = 100;
        Health.Value = CharacterStats.HP;
        Position = new Vector2(100, 100);
        AnimatedSprite.Play("idle_right");
    }

    public void HasInventorySpace(bool status)
    {
        if (status == false)
        {
            InventoryFull = true;
        }
        else
        {
            InventoryFull = false;
        }
    }
}
