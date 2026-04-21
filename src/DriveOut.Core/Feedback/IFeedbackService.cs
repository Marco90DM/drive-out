namespace DriveOut.Core.Feedback;

public interface IFeedbackService
{
    void Trigger(FeedbackEvent feedbackEvent);
    void Trigger(FeedbackEvent feedbackEvent, float intensity);
}
