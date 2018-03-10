namespace Bisner.Mobile.Core.Communication
{
    public enum ApiPriority
    {
        Explicit = 0,
        Speculative = 10,
        Background = 20,
        UserInitiated = 100
    }
}