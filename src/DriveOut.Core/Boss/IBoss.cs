namespace DriveOut.Core.Boss;

public interface IBoss
{
    string Name { get; }
    BossPhase Phase { get; }
    float MaxHealth { get; }
    float CurrentHealth { get; }
    bool IsAlive { get; }

    event Action<BossPhase> OnPhaseChanged;
    event Action OnDefeated;

    void Engage();
    void Update(float deltaTime);
    void TakeDamage(float amount);
    void Reset();
}
