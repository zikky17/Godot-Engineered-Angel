using EngineeredAngel.Loot;
using EngineeredAngel.Services;
using EngineeredAngel.Stats;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Gorgon : CharacterBody2D
{
    [Export] public int Speed = 50;
    [Export] public int ChargeSpeed = 75;
    public bool GorgonCharge = false;
    public bool PlayerInRange = false;
    public bool IsAttacking = false;
    public bool IsDead = false;
    public Zikky zikky;
    public ProgressBar Health;
    public bool HasTakenDamage = false;
    private Area2D _hitbox;
    private Area2D _territory;
    private AudioStreamPlayer _audioPlayer;
    private AudioStream chargeSound;
    private AudioStream deathSound;
    private Label _damageLabel;
    private Timer _damageLabelTimer;
    private LevelUpService _levelUpService;

    public Vector2 LastDirection { get; set; } = Vector2.Zero;
    public AnimatedSprite2D AnimatedSprite { get; private set; }
    private Random random = new Random();

    public Timer Direction_Change_Timer { get; private set; }

    private RewardService _rewardService;

    public override void _Ready()
    {

        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        Direction_Change_Timer = new Timer();
        AddChild(Direction_Change_Timer);
        Direction_Change_Timer.WaitTime = 2;
        Direction_Change_Timer.OneShot = false;
        Direction_Change_Timer.Connect(Timer.SignalName.Timeout, Callable.From(OnDirectionChangeTimeout));
        Direction_Change_Timer.Start();

        _damageLabel = GetNode<Label>("DamageLabel");
        _damageLabel.Visible = false;
        _damageLabelTimer = new Timer();
        _damageLabelTimer.WaitTime = 0.5f;
        _damageLabelTimer.OneShot = true;
        AddChild(_damageLabelTimer);

        _damageLabelTimer.Connect("timeout", new Callable(this, nameof(HideDamageLabel)));

        Health = GetNode<ProgressBar>("healthbar");
        Health.MaxValue = 100;
        Health.Value = Health.MaxValue;

        _hitbox = GetNode<Area2D>("Gorgon_Hitbox");
        _territory = GetNode<Area2D>("Territory");
        _audioPlayer = GetNode<AudioStreamPlayer>("GorgonSounds");
        chargeSound = (AudioStream)GD.Load("res://Assets/SoundEffects/ScarySounds/Gorgon_Laugh.mp3");
        deathSound = (AudioStream)GD.Load("res://Assets/SoundEffects/ScarySounds/Gorgon_Death.mp3");

        AnimatedSprite.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(OnAnimationFinished));

        _territory.Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnTerritoryBodyEntered)));
        _territory.Connect(Area2D.SignalName.BodyExited, new Callable(this, nameof(OnTerritoryBodyExited)));

        _hitbox.Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnHitboxBodyEntered)));
        _hitbox.Connect(Area2D.SignalName.BodyExited, new Callable(this, nameof(OnHitboxBodyExited)));

        zikky = GetNode<Zikky>("../Zikky");

        _levelUpService = new LevelUpService();
        _rewardService = new RewardService(zikky, _levelUpService);

        AddToGroup("Enemy");
    }

    public void PickRandomDirection()
    {
        LastDirection = new Vector2(
            random.Next(-1, 2),
            random.Next(-1, 2)
        );

        if (LastDirection == Vector2.Zero)
        {
            PickRandomDirection();
        }

    }

    public override void _PhysicsProcess(double delta)
    {
        HasTakenDamage = false;
        OnPlayerAttacked();

        if (GorgonCharge && zikky != null)
        {
            var playerDirection = (zikky.Position - Position).Normalized();
            Position += playerDirection * ChargeSpeed * (float)delta;
            UpdateAnimation(playerDirection, true);
        }
        else
        {
            Velocity = LastDirection * Speed;
        }

        MoveAndSlide();

        if (Health.Value <= 0)
        {
            Speed = 0;
            GorgonCharge = false;
            Velocity = LastDirection * Speed;
            Die();
        }

    }

    private void OnDirectionChangeTimeout()
    {
        PickRandomDirection();
        Direction_Change_Timer.Start();
    }

    public void UpdateAnimation(Vector2 position, bool? charge)
    {

        if (PlayerInRange)
        {
            AnimatedSprite.Play("fight");
            AnimatedSprite.FlipH = position.X < 0;
            AnimatedSprite.FlipV = false;
        }
        else if (!PlayerInRange)
        {
            AnimatedSprite.Play("move_right");
            AnimatedSprite.FlipH = false;
            AnimatedSprite.FlipV = false;
        }

        if (position.X == 0)
        {
            AnimatedSprite.Play("move_left");
            AnimatedSprite.FlipH = position.X < 0;
        }
    }


    private void OnHitboxBodyEntered(Node body)
    {
        if (body is CharacterBody2D characterBody && body.IsInGroup("Player"))
        {
            PlayerInRange = true;
            IsAttacking = true;
            UpdateAnimation(Position, null);
        }
    }


    private void OnHitboxBodyExited(Node body)
    {
        if (body is CharacterBody2D characterBody && body.IsInGroup("Player"))
        {
            PlayerInRange = false;
            UpdateAnimation(Position, null);
        }
    }

    private void OnTerritoryBodyEntered(Node body)
    {
        if (body is CharacterBody2D characterBody && body.IsInGroup("Player"))
        {
            GorgonCharge = true;
            _audioPlayer.Stream = chargeSound;
            _audioPlayer.Play();
        }
    }

    private void OnTerritoryBodyExited(Node body)
    {
        if (body is CharacterBody2D characterBody && body.IsInGroup("Player"))
        {
            GorgonCharge = false;
            PickRandomDirection();
        }
    }

    private void OnPlayerAttacked()
    {
        if (PlayerInRange && !HasTakenDamage && zikky.IsAttacking)
        {
            var damage = zikky.CharacterStats.DealDamage();
            Health.Value -= damage;
            HasTakenDamage = true;
            GD.Print($"Gorgon took {damage} damage, remaining health: " + Health.Value);
            ShowDamage(damage);
            zikky.IsAttacking = false;
            HasTakenDamage = false;
        }
    }

    public void UpdateHealthBar()
    {
        Health.Value = 50;
        Health.Visible = true;
    }

    public void Die()
    {
        IsDead = true;
        AnimatedSprite.Play("die");
        _audioPlayer.Stream = deathSound;
        _audioPlayer.Play();
    }

    private void OnAnimationFinished()
    {
        if (IsDead)
        {
            QueueFree();
            _rewardService.GrantRewards(GenerateLoot(), 100);
            _levelUpService.GetExperience(100);
        }
    }

    private void ShowDamage(int damage)
    {
        _damageLabel.Text = "-" + damage.ToString();
        _damageLabel.Visible = true;
        _damageLabelTimer.Start();
    }

    private void HideDamageLabel()
    {
        _damageLabel.Visible = false;
    }

    private int GenerateLoot()
    {
        var loot = new Loot();
        return loot.Gold = 5;
    }
}
