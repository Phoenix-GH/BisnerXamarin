namespace Bisner.Mobile.Core.Communication
{
    public interface IApiService<out TApi>
    {
        TApi GetApi(ApiPriority priority);

        TApi Explicit { get; }
        TApi Speculative { get; }
        TApi Background { get; }
        TApi UserInitiated { get; }
    }
}
