namespace Bisner.Mobile.Core.Service
{
    public interface IAnalyticsService
    {
        void Initialize();
        void SendScreen(string screenName);
        void SendEvent(string gaEventCategory, string @event);
        void SendException(string exceptionInfo, bool isFatalException);
        void AddTracker(string trackingId);
    }
}
