using EngineeredAngel.Loot;
using EngineeredAngel.Services;
using EngineeredAngel.Stats;
using Godot;
using System;

public partial class Gorgon : CharacterBody2D
{
    [Export] public int Speed = 50;
    [Export] public int ChargeSpeed = 75;
    public bool GorgonCharge = false;
    public bool PlayerInRange = false;
    public bool IsAttacking = false;
    public bool IsDead = false;
    private Zikky _zikky;
    private ProgressBar _health;
    private Area2D _hitbox;
    private Area2D _territory;
    private AudioStreamPlayer _audioPlayer;
    private AudioStream _chargeSound;
    private AudioStream _deathSound;
    private Label _damageLabel;
    private Timer _damageLabelTimer;
    private Random _random = new Random();
    private RewardService _rewardService;

    public Vector2 LastDirection { get; set; } = Vector2.Zero;
    public AnimatedSprite2D AnimatedSprite { get; private set; }
    private Timer _directionChangeTimer;

    public override void _Ready()
    {
        _zikky = GetNode<Zikky>("../Zikky");
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        AnimatedSprite.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));

        _directionChangeTimer = new Timer { WaitTime = 2, OneShot = false };
        AddChild(_directionChangeTimer);
        _directionChangeTimer.Connect("timeout", new Callable(this, nameof(OnDirectionChangeTimeout)));
        _directionChangeTimer.Start();

        _damageLabel = GetNode<Label>("DamageLabel");
        _damageLabel.Visible = false;
        _damageLabelTimer = new Timer { WaitTime = 0.5f, OneShot = true };
        AddChild(_damageLabelTimer);
        _damageLabelTimer.Connect("timeout", new Callable(this, nameof(HideDamageLabel)));

        _health = GetNode<ProgressBar>("healthbar");
        _health.MaxValue = 100;
        _health.Value = _health.MaxValue;

        _hitbox = GetNode<Area2D>("Gorgon_Hitbox");
        _territory = GetNode<Area2D>("Territory");
        _audioPlayer = GetNode<AudioStreamPlayer>("GorgonSounds");
        _chargeSound = GD.Load<AudioStream>("res://Assets/SoundEffects/ScarySounds/Gorgon_Laugh.mp3");
        _deathSound = GD.Load<AudioStream>("res://Assets/SoundEffects/ScarySounds/Gorgon_Death.mp3");

        _hitbox.Connect("body_entered", new Callable(this, nameof(OnHitboxBodyEntered)));
        _hitbox.Connect("body_exited", new Callable(this, nameof(OnHitboxBodyExited)));
        _territory.Connect("body_entered", new Callable(this, nameof(OnTerritoryBodyEntered)));
        _territory.Connect("body_exited", new Callable(this, nameof(OnTerritoryBodyExited)));


        var levelUpService = new LevelUpService();
        _rewardService = new RewardService(_zikky, levelUpService);

        AddToGroup("Enemy");
    }

    private void OnDirectionChangeTimeout()
    {
        LastDirection = new Vector2(_random.Next(-1, 2), _random.Next(-1, 2));
        if (LastDirection == Vector2.Zero)
        {
            OnDirectionChangeTimeout();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead) return;
        if (GorgonCharge && _zikky != null)
        {
            var playerDirection = (_zikky.Position - Position).Normalized();
            Position += playerDirection * ChargeSpeed * (float)delta;
            UpdateAnimation(playerDirection);
        }
        else
        {
            Velocity = LastDirection * Speed;
        }

        MoveAndSlide();

        if (PlayerInRange && _zikky.IsAttacking)
        {
            OnPlayerAttacked();
        }
    }

    private void OnHitboxBodyEntered(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            PlayerInRange = true;
            IsAttacking = true;
            UpdateAnimation(Position);
        }
    }

    private void OnHitboxBodyExited(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            PlayerInRange = false;
            UpdateAnimation(Position);
        }
    }

    private void OnTerritoryBodyEntered(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            GorgonCharge = true;
            _audioPlayer.Stream = _chargeSound;
            _audioPlayer.Play();
        }
    }

    private void OnTerritoryBodyExited(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            GorgonCharge = false;
            LastDirection = Vector2.Zero;
            OnDirectionChangeTimeout();
        }
    }

    private void OnPlayerAttacked()
    {
        if (IsDead || !PlayerInRange || !_zikky.IsAttacking) return;

        var damage = _zikky.CharacterStats.DealDamage();
        _health.Value -= damage;
        ShowDamage(damage);

        if (_health.Value <= 0 && !IsDead)
        {
            Die();
        }
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (IsDead) return;
        if (PlayerInRange)
        {
            AnimatedSprite.Play("fight");
            AnimatedSprite.FlipH = direction.X < 0;
        }
        else
        {
            AnimatedSprite.Play("move_right");
        }
    }

    private void Die()
    {
        IsDead = true;
        Speed = 0;
        GorgonCharge = false;
        PlayerInRange = false;
        AnimatedSprite.Play("die");
        _audioPlayer.Stream = _deathSound;
        _audioPlayer.Play();
    }

    private void ShowDamage(int damage)
    {
        if (IsDead) return;
        _damageLabel.Text = $"-{damage}";
        _damageLabel.Visible = true;
        _damageLabelTimer.Start();
    }

    private void HideDamageLabel()
    {
        _damageLabel.Visible = false;
    }

    private void OnAnimationFinished()
    {
        if (IsDead && AnimatedSprite.Animation == "die")
        {
            QueueFree();
            _rewardService.GrantRewards(GenerateLoot(), 100);
        }
    }

    private int GenerateLoot()
    {
        return new Loot().Gold = 5;
    }
}
