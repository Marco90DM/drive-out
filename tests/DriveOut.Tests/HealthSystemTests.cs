using DriveOut.Core.Health;
using FluentAssertions;

namespace DriveOut.Tests;

public class HealthSystemTests
{
    [Fact]
    public void Constructor_WithValidMaxHealth_SetsCurrentHealthToMax()
    {
        var sut = new HealthSystem(100f);
        sut.CurrentHealth.Should().Be(100f);
        sut.IsAlive.Should().BeTrue();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-10f)]
    public void Constructor_WithInvalidMaxHealth_Throws(float max)
    {
        var act = () => new HealthSystem(max);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TakeDamage_ReducesCurrentHealth()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(30f);
        sut.CurrentHealth.Should().Be(70f);
    }

    [Fact]
    public void TakeDamage_WhenFatal_SetsCurrentHealthToZeroAndFiresOnDeath()
    {
        var sut = new HealthSystem(100f);
        var deathFired = false;
        sut.OnDeath += () => deathFired = true;

        sut.TakeDamage(200f);

        sut.CurrentHealth.Should().Be(0f);
        sut.IsAlive.Should().BeFalse();
        deathFired.Should().BeTrue();
    }

    [Fact]
    public void TakeDamage_WhenAlreadyDead_DoesNothing()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(100f);
        var deathCount = 0;
        sut.OnDeath += () => deathCount++;

        sut.TakeDamage(50f);

        deathCount.Should().Be(0);
        sut.CurrentHealth.Should().Be(0f);
    }

    [Fact]
    public void TakeDamage_FiresOnDamagedWithCorrectAmount()
    {
        var sut = new HealthSystem(100f);
        float received = 0f;
        sut.OnDamaged += a => received = a;

        sut.TakeDamage(25f);

        received.Should().Be(25f);
    }

    [Fact]
    public void Heal_IncreasesCurrentHealth()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(50f);
        sut.Heal(20f);
        sut.CurrentHealth.Should().Be(70f);
    }

    [Fact]
    public void Heal_DoesNotExceedMaxHealth()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(10f);
        sut.Heal(999f);
        sut.CurrentHealth.Should().Be(100f);
    }

    [Fact]
    public void Heal_WhenDead_DoesNothing()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(100f);
        sut.Heal(50f);
        sut.CurrentHealth.Should().Be(0f);
    }

    [Fact]
    public void Reset_RestoresFullHealth()
    {
        var sut = new HealthSystem(100f);
        sut.TakeDamage(80f);
        sut.Reset();
        sut.CurrentHealth.Should().Be(100f);
        sut.IsAlive.Should().BeTrue();
    }
}
