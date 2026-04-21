using DriveOut.Systems.Run;
using FluentAssertions;

namespace DriveOut.Tests;

public class RunManagerTests
{
    [Fact]
    public void InitialState_IsNew()
    {
        var sut = new RunManager();
        sut.State.Should().Be(RunState.New);
    }

    [Fact]
    public void StartRun_TransitionsToRunning_AndFiresEvent()
    {
        var sut = new RunManager();
        var fired = false;
        sut.OnRunStarted += () => fired = true;

        sut.StartRun();

        sut.State.Should().Be(RunState.Running);
        fired.Should().BeTrue();
    }

    [Fact]
    public void StartRun_WhenNotNew_Throws()
    {
        var sut = new RunManager();
        sut.StartRun();

        var act = () => sut.StartRun();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Pause_FromRunning_TransitionsToPaused_AndFiresEvent()
    {
        var sut = new RunManager();
        sut.StartRun();
        var fired = false;
        sut.OnRunPaused += () => fired = true;

        sut.Pause();

        sut.State.Should().Be(RunState.Paused);
        fired.Should().BeTrue();
    }

    [Fact]
    public void Pause_WhenNotRunning_Throws()
    {
        var sut = new RunManager();
        var act = () => sut.Pause();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Resume_FromPaused_TransitionsToRunning_AndFiresEvent()
    {
        var sut = new RunManager();
        sut.StartRun();
        sut.Pause();
        var fired = false;
        sut.OnRunResumed += () => fired = true;

        sut.Resume();

        sut.State.Should().Be(RunState.Running);
        fired.Should().BeTrue();
    }

    [Fact]
    public void Resume_WhenNotPaused_Throws()
    {
        var sut = new RunManager();
        sut.StartRun();
        var act = () => sut.Resume();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void TriggerGameOver_FromRunning_TransitionsToGameOver_AndFiresEvent()
    {
        var sut = new RunManager();
        sut.StartRun();
        var fired = false;
        sut.OnGameOver += () => fired = true;

        sut.TriggerGameOver();

        sut.State.Should().Be(RunState.GameOver);
        fired.Should().BeTrue();
    }

    [Fact]
    public void TriggerGameOver_FromPaused_TransitionsToGameOver()
    {
        var sut = new RunManager();
        sut.StartRun();
        sut.Pause();

        sut.TriggerGameOver();

        sut.State.Should().Be(RunState.GameOver);
    }

    [Fact]
    public void TriggerGameOver_FromNew_Throws()
    {
        var sut = new RunManager();
        var act = () => sut.TriggerGameOver();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Reset_FromGameOver_TransitionsToNew_AndAllowsNewRun()
    {
        var sut = new RunManager();
        sut.StartRun();
        sut.TriggerGameOver();
        var fired = false;
        sut.OnRunReset += () => fired = true;

        sut.Reset();

        sut.State.Should().Be(RunState.New);
        fired.Should().BeTrue();
        var act = () => sut.StartRun();
        act.Should().NotThrow();
    }

    [Fact]
    public void FullLifecycle_NewRunPauseResumeGameOverReset_Works()
    {
        var sut = new RunManager();

        sut.StartRun();
        sut.State.Should().Be(RunState.Running);

        sut.Pause();
        sut.State.Should().Be(RunState.Paused);

        sut.Resume();
        sut.State.Should().Be(RunState.Running);

        sut.TriggerGameOver();
        sut.State.Should().Be(RunState.GameOver);

        sut.Reset();
        sut.State.Should().Be(RunState.New);
    }
}
