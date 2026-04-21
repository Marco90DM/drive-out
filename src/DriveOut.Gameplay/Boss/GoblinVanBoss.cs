using DriveOut.Core.Boss;

namespace DriveOut.Gameplay.Boss;

public sealed class GoblinVanBoss : IBoss
{
    private readonly float _chaseDuration;
    private readonly float _attackDuration;
    private readonly float _retreatDuration;
    private float _phaseTimer;

    public string Name => "Goblin Van";
    public BossPhase Phase { get; private set; } = BossPhase.Idle;
    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0f;

    public event Action<BossPhase>? OnPhaseChanged;
    public event Action? OnDefeated;

    public GoblinVanBoss(
        float maxHealth = 150f,
        float chaseDuration = 4f,
        float attackDuration = 2f,
        float retreatDuration = 3f)
    {
        if (maxHealth <= 0f) throw new ArgumentOutOfRangeException(nameof(maxHealth));

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _chaseDuration = chaseDuration;
        _attackDuration = attackDuration;
        _retreatDuration = retreatDuration;
    }

    public void Engage()
    {
        if (!IsAlive || Phase != BossPhase.Idle) return;
        TransitionTo(BossPhase.Engaging);
    }

    public void Update(float deltaTime)
    {
        if (!IsAlive || Phase is BossPhase.Idle or BossPhase.Defeated) return;

        _phaseTimer -= deltaTime;
        if (_phaseTimer > 0f) return;

        switch (Phase)
        {
            case BossPhase.Engaging:
                TransitionTo(BossPhase.Attacking);
                break;
            case BossPhase.Attacking:
                TransitionTo(BossPhase.Retreating);
                break;
            case BossPhase.Retreating:
                TransitionTo(BossPhase.Engaging);
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || amount <= 0f) return;

        CurrentHealth = Math.Max(0f, CurrentHealth - amount);
        if (!IsAlive)
        {
            TransitionTo(BossPhase.Defeated);
            OnDefeated?.Invoke();
        }
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
        Phase = BossPhase.Idle;
        _phaseTimer = 0f;
    }

    private void TransitionTo(BossPhase next)
    {
        Phase = next;
        _phaseTimer = next switch
        {
            BossPhase.Engaging   => _chaseDuration,
            BossPhase.Attacking  => _attackDuration,
            BossPhase.Retreating => _retreatDuration,
            _ => 0f
        };
        OnPhaseChanged?.Invoke(next);
    }
}
