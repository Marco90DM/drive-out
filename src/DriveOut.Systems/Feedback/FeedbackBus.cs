using DriveOut.Core.Feedback;

namespace DriveOut.Systems.Feedback;

public sealed class FeedbackBus : IFeedbackService
{
    public event Action<FeedbackEvent, float>? OnFeedback;

    public void Trigger(FeedbackEvent feedbackEvent) => Trigger(feedbackEvent, 1f);

    public void Trigger(FeedbackEvent feedbackEvent, float intensity)
    {
        if (intensity < 0f) throw new ArgumentOutOfRangeException(nameof(intensity));
        OnFeedback?.Invoke(feedbackEvent, intensity);
    }
}
