using DriveOut.Core.Boss;

namespace DriveOut.Gameplay.Boss;

public sealed class VentMonsterBoss : IBoss
{
    private readonly float _emergeDuration;
    private readonly float _attackDuration;
    private readonly float _retreatDuration;
    private float _phaseTimer;
    private int _attackCycles;

    public string Name => "Vent Monster";
    public BossPhase Phase { get; private set; } = BossPhase.Idle;
    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0f;
    public int MaxAttackCycles { get; }

    public event Action<BossPhase>? OnPhaseChanged;
    public event Action? OnDefeated;

    public VentMonsterBoss(
        float maxHealth = 100f,
        float emergeDuration = 2f,
        float attackDuration = 3f,
        float retreatDuration = 2f,
        int maxAttackCycles = 3)
    {
        if (maxHealth <= 0f) throw new ArgumentOutOfRangeException(nameof(maxHealth));
        if (maxAttackCycles <= 0) throw new ArgumentOutOfRangeException(nameof(maxAttackCycles));

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _emergeDuration = emergeDuration;
        _attackDuration = attackDuration;
        _retreatDuration = retreatDuration;
        MaxAttackCycles = maxAttackCycles;
    }

    public void Engage()
    {
        if (!IsAlive || Phase != BossPhase.Idle) return;
        _attackCycles = 0;
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
                _attackCycles++;
                TransitionTo(BossPhase.Retreating);
                break;
            case BossPhase.Retreating:
                // After max cycles the boss disappears until defeated
                if (_attackCycles < MaxAttackCycles)
                    TransitionTo(BossPhase.Engaging);
                else
                    TransitionTo(BossPhase.Idle);
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
        _attackCycles = 0;
    }

    private void TransitionTo(BossPhase next)
    {
        Phase = next;
        _phaseTimer = next switch
        {
            BossPhase.Engaging   => _emergeDuration,
            BossPhase.Attacking  => _attackDuration,
            BossPhase.Retreating => _retreatDuration,
            _ => 0f
        };
        OnPhaseChanged?.Invoke(next);
    }
}
