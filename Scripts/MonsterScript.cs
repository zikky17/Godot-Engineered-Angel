using EngineeredAngel.Loot;
using EngineeredAngel.Models.QuestModels;
using EngineeredAngel.Monsters;
using EngineeredAngel.Services;
using Godot;
using System;
using System.Collections.Generic;

public partial class MonsterScript : CharacterBody2D
{
    [Export] public int Speed = 100;
    [Export] public int ChargeSpeed = 175;

    public bool MonsterCharge = false;
    public bool PlayerInRange = false;
    public bool IsAttacking = false;
    public bool IsDead = false;

    private Player _Player;
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
    private QuestService _questService = new();
    private QuestMenu _questMenu;

    public List<LootItem> LootTable { get; private set; }
    public Vector2 LastDirection { get; set; } = Vector2.Zero;
    public AnimatedSprite2D AnimatedSprite { get; private set; }
    private Timer _directionChangeTimer;

    private float _lastAttackTime = -2f;
    private const float AttackCooldown = 2f;

    public override void _Ready()
    {
        var monsterName = Name;
        var monsterService = new MonsterGeneratorService();
        var monster = monsterService.RetrieveMonsterProfile(monsterName);
        var monsterData = monster.ReturnMonsterData();

        LootTable = monsterData.LootTable;
        _chargeSound = monsterData.ChargeSound;
        _deathSound = monsterData.DeathSound;

        _Player = GetNode<Player>("../Player");
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        AnimatedSprite.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));

        _directionChangeTimer = new Timer { WaitTime = 2, OneShot = false };
        AddChild(_directionChangeTimer);
        _directionChangeTimer.Connect("timeout", new Callable(this, nameof(OnDirectionChangeTimeout)));
        _directionChangeTimer.Start();

        _questMenu = GetNodeOrNull<QuestMenu>("../Player/CharacterMenus/QuestMenu");

        _damageLabel = GetNode<Label>("DamageLabel");
        _damageLabel.Visible = false;
        _damageLabelTimer = new Timer { WaitTime = 0.5f, OneShot = true };
        AddChild(_damageLabelTimer);
        _damageLabelTimer.Connect("timeout", new Callable(this, nameof(HideDamageLabel)));

        _health = GetNode<ProgressBar>("Healthbar");
        _health.MaxValue = 100;
        _health.Value = _health.MaxValue;

        _hitbox = GetNode<Area2D>("Hitbox");
        _territory = GetNode<Area2D>("Territory");
        _audioPlayer = GetNode<AudioStreamPlayer>("BattleCry");

        _hitbox.Connect("body_entered", new Callable(this, nameof(OnHitboxBodyEntered)));
        _hitbox.Connect("body_exited", new Callable(this, nameof(OnHitboxBodyExited)));
        _territory.Connect("body_entered", new Callable(this, nameof(OnTerritoryBodyEntered)));
        _territory.Connect("body_exited", new Callable(this, nameof(OnTerritoryBodyExited)));

        var levelUpService = new LevelUpService();
        _rewardService = new RewardService(_Player, levelUpService);

        AddToGroup("Enemy");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead) return;

        if (MonsterCharge && _Player != null)
        {
            var playerDirection = (_Player.Position - Position).Normalized();
            Position += playerDirection * ChargeSpeed * (float)delta;
            UpdateAnimation(playerDirection);
        }
        else
        {
            Velocity = LastDirection * Speed;
        }

        MoveAndSlide();

        if (PlayerInRange)
        {
            TryDamagePlayer();
        }

        if (PlayerInRange && _Player.IsAttacking)
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
            IsAttacking = false;
            UpdateAnimation(Position);
        }
    }

    private void TryDamagePlayer()
    {
        if (_Player == null || _Player.IsDead) return;

        float currentTime = Time.GetTicksMsec() / 1000f;
        if (currentTime - _lastAttackTime >= AttackCooldown)
        {
            _lastAttackTime = currentTime;

            var monsterProfile = new Gorgon().ReturnMonsterData();
            var baseDamage = monsterProfile.DealDamage();

            int finalDamage = _Player.CharacterStats.CalculateDamage(baseDamage);
            _Player.CharacterStats.HP -= finalDamage;
            _Player.Health.Value = _Player.CharacterStats.HP;

            _Player.ShowCombatText(finalDamage, null);

            GD.Print($"Player took {finalDamage} damage from {Name}. Remaining HP: {_Player.CharacterStats.HP}");

            if (_Player.CharacterStats.HP <= 0 && !_Player.IsDead)
            {
                _Player.Die();
            }
        }
    }

    private void OnTerritoryBodyEntered(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            MonsterCharge = true;
            _audioPlayer.Stream = _chargeSound;
        }
    }

    private void OnTerritoryBodyExited(Node body)
    {
        if (IsDead) return;
        if (body.IsInGroup("Player"))
        {
            MonsterCharge = false;
            LastDirection = Vector2.Zero;
            OnDirectionChangeTimeout();
        }
    }

    private void OnPlayerAttacked()
    {
        if (IsDead || !PlayerInRange || !_Player.IsAttacking) return;

        var damage = _Player.CharacterStats.DealDamage();
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
        MonsterCharge = false;
        PlayerInRange = false;
        AnimatedSprite.Play("die");
        _audioPlayer.Stream = _deathSound;

        Dictionary<string, QuestData> quests = _questService.LoadAllQuests();
        if (quests != null && quests.Count > 0)
        {
            foreach (var quest in quests.Values)
            {
                var monster = quest.Monster;
                if (monster != null)
                {
                    quest.KillCount--;
                    if (quest.KillCount <= 0)
                    {
                        quest.Description = $"Quest Completed! Return to {quest.NPC}";
                        quest.IsCompleted = true;
                        quest.KillCount = null;
                    }

                    _questService.SaveQuest(quest.Name, quest.Description, quest.KillCount, quest.Monster,
                        quest.NPC, quest.IsCompleted, quest.QuestReward.Gold,
                        quest.QuestReward.Experience, quest.QuestReward.ItemReward);

                    _questMenu?.UpdateQuestsUI();
                    GD.Print("Quest updated");
                }
            }
        }
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
            DropLoot();
            QueueFree();
        }
    }

    private void DropLoot()
    {
        List<string> droppedItems = new List<string>();

        foreach (var item in LootTable)
        {
            float roll = (float)_random.NextDouble();
            if (roll <= item.DropChance)
            {
                GD.Print($"Rolled: {roll:F2} for {item.Name} (DropChance: {item.DropChance:F2}) - SUCCESS");
                droppedItems.Add($"{item.Name} x{item.Quantity}");

                if (item.Name == "Gold")
                {
                    _rewardService.GrantRewards(item.Quantity, null);
                }
                else if (item.Name == "Experience")
                {
                    _rewardService.GrantRewards(null, item.Quantity);
                }
                else
                {
                    SpawnLoot(item);
                }
            }
            else
            {
                GD.Print($"Rolled: {roll:F2} for {item.Name} (DropChance: {item.DropChance:F2}) - FAIL");
            }
        }

        foreach (var dropped in droppedItems)
        {
            GD.Print("Dropped: " + dropped);
        }
    }

    private void SpawnLoot(LootItem item)
    {
        if (item.Name == "Iron Sword")
        {
            var lootNode = GD.Load<PackedScene>("res://Loot/IronSword.tscn").Instantiate<Loot>();
            lootNode.Name = item.Name;
            lootNode.Type = item.Type;
            lootNode.Quantity = item.Quantity;
            lootNode.Position = GlobalPosition;
            GetParent().AddChild(lootNode);
        }

        if (item.Name == "Healing Potion")
        {
            var lootNode = GD.Load<PackedScene>("res://Scenes/Consumables/health_potion.tscn").Instantiate<Area2D>();
            lootNode.Name = item.Name;
            lootNode.Position = GlobalPosition;
            GetParent().AddChild(lootNode);
        }
    }

    private void OnDirectionChangeTimeout()
    {
        LastDirection = new Vector2(_random.Next(-1, 2), _random.Next(-1, 2));
        if (LastDirection == Vector2.Zero)
        {
            OnDirectionChangeTimeout();
        }
    }
}
