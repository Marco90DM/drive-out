namespace DriveOut.Core.Health;

public interface IHealthSystem
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    bool IsAlive { get; }

    event Action<float> OnDamaged;
    event Action<float> OnHealed;
    event Action OnDeath;

    void TakeDamage(float amount);
    void Heal(float amount);
    void Reset();
}
