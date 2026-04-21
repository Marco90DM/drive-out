namespace DriveOut.Core.Health;

public sealed class HealthSystem : IHealthSystem
{
    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0f;

    public event Action<float>? OnDamaged;
    public event Action<float>? OnHealed;
    public event Action? OnDeath;

    public HealthSystem(float maxHealth)
    {
        if (maxHealth <= 0f)
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "MaxHealth must be greater than zero.");

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || amount <= 0f) return;

        CurrentHealth = Math.Max(0f, CurrentHealth - amount);
        OnDamaged?.Invoke(amount);

        if (!IsAlive)
            OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        if (!IsAlive || amount <= 0f) return;

        var actual = Math.Min(amount, MaxHealth - CurrentHealth);
        if (actual <= 0f) return;

        CurrentHealth += actual;
        OnHealed?.Invoke(actual);
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
    }
}
