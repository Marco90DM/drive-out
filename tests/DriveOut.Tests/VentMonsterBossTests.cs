using DriveOut.Core.Boss;
using DriveOut.Gameplay.Boss;

namespace DriveOut.Tests;

public class VentMonsterBossTests
{
    private static VentMonsterBoss Default() =>
        new(maxHealth: 100f, emergeDuration: 2f, attackDuration: 3f, retreatDuration: 2f, maxAttackCycles: 2);

    [Fact]
    public void InitialState_IsIdleAndFullHealth()
    {
        var sut = Default();
        sut.Phase.Should().Be(BossPhase.Idle);
        sut.CurrentHealth.Should().Be(100f);
    }

    [Fact]
    public void Engage_TransitionsToEngaging()
    {
        var sut = Default();
        sut.Engage();
        sut.Phase.Should().Be(BossPhase.Engaging);
    }

    [Fact]
    public void Update_FullAttackCycle_ThenReturnToIdle()
    {
        var sut = Default();
        sut.Engage();

        // Cycle 1
        sut.Update(2.1f); // Engaging → Attacking
        sut.Phase.Should().Be(BossPhase.Attacking);
        sut.Update(3.1f); // Attacking → Retreating
        sut.Update(2.1f); // Retreating → Engaging (cycle 1 done, max=2)

        // Cycle 2
        sut.Update(2.1f); // Engaging → Attacking
        sut.Update(3.1f); // Attacking → Retreating
        sut.Update(2.1f); // Retreating → Idle (max cycles reached)

        sut.Phase.Should().Be(BossPhase.Idle);
    }

    [Fact]
    public void TakeDamage_WhenFatal_Defeats()
    {
        var sut = Default();
        sut.Engage();
        var defeated = false;
        sut.OnDefeated += () => defeated = true;

        sut.TakeDamage(200f);

        sut.Phase.Should().Be(BossPhase.Defeated);
        defeated.Should().BeTrue();
    }

    [Fact]
    public void Reset_RestoresStateAndAttackCycles()
    {
        var sut = Default();
        sut.Engage();
        sut.TakeDamage(50f);
        sut.Reset();

        sut.Phase.Should().Be(BossPhase.Idle);
        sut.CurrentHealth.Should().Be(100f);
        // After reset can re-engage and run cycles again
        sut.Engage();
        sut.Phase.Should().Be(BossPhase.Engaging);
    }
}
