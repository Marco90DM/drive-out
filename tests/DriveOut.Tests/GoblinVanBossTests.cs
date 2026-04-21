using DriveOut.Core.Boss;
using DriveOut.Gameplay.Boss;

namespace DriveOut.Tests;

public class GoblinVanBossTests
{
    private static GoblinVanBoss Default() =>
        new(maxHealth: 150f, chaseDuration: 4f, attackDuration: 2f, retreatDuration: 3f);

    [Fact]
    public void InitialState_IsIdleAndFullHealth()
    {
        var sut = Default();
        sut.Phase.Should().Be(BossPhase.Idle);
        sut.CurrentHealth.Should().Be(150f);
        sut.IsAlive.Should().BeTrue();
    }

    [Fact]
    public void Engage_TransitionsToEngaging()
    {
        var sut = Default();
        sut.Engage();
        sut.Phase.Should().Be(BossPhase.Engaging);
    }

    [Fact]
    public void Engage_WhenAlreadyEngaging_DoesNothing()
    {
        var sut = Default();
        sut.Engage();
        sut.Engage();
        sut.Phase.Should().Be(BossPhase.Engaging);
    }

    [Fact]
    public void Update_AfterChaseDuration_TransitionsToAttacking()
    {
        var sut = Default();
        sut.Engage();
        sut.Update(4.1f);
        sut.Phase.Should().Be(BossPhase.Attacking);
    }

    [Fact]
    public void Update_AfterAttackDuration_TransitionsToRetreating()
    {
        var sut = Default();
        sut.Engage();
        sut.Update(4.1f);
        sut.Update(2.1f);
        sut.Phase.Should().Be(BossPhase.Retreating);
    }

    [Fact]
    public void Update_AfterRetreatDuration_CyclesBackToEngaging()
    {
        var sut = Default();
        sut.Engage();
        sut.Update(4.1f);
        sut.Update(2.1f);
        sut.Update(3.1f);
        sut.Phase.Should().Be(BossPhase.Engaging);
    }

    [Fact]
    public void OnPhaseChanged_FiresOnEachTransition()
    {
        var sut = Default();
        var phases = new List<BossPhase>();
        sut.OnPhaseChanged += p => phases.Add(p);

        sut.Engage();
        sut.Update(4.1f);
        sut.Update(2.1f);

        phases.Should().ContainInOrder(BossPhase.Engaging, BossPhase.Attacking, BossPhase.Retreating);
    }

    [Fact]
    public void TakeDamage_ReducesHealth()
    {
        var sut = Default();
        sut.TakeDamage(50f);
        sut.CurrentHealth.Should().Be(100f);
    }

    [Fact]
    public void TakeDamage_WhenFatal_TransitionsToDefeatedAndFiresEvent()
    {
        var sut = Default();
        sut.Engage();
        var defeated = false;
        sut.OnDefeated += () => defeated = true;

        sut.TakeDamage(200f);

        sut.Phase.Should().Be(BossPhase.Defeated);
        sut.IsAlive.Should().BeFalse();
        defeated.Should().BeTrue();
    }

    [Fact]
    public void Update_WhenDefeated_DoesNotTransition()
    {
        var sut = Default();
        sut.TakeDamage(200f);
        sut.Update(10f);
        sut.Phase.Should().Be(BossPhase.Defeated);
    }

    [Fact]
    public void Reset_RestoresFullHealthAndIdleState()
    {
        var sut = Default();
        sut.Engage();
        sut.TakeDamage(80f);
        sut.Reset();

        sut.Phase.Should().Be(BossPhase.Idle);
        sut.CurrentHealth.Should().Be(150f);
        sut.IsAlive.Should().BeTrue();
    }
}
