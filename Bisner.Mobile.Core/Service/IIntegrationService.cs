using System.Threading.Tasks;

namespace Bisner.Mobile.Core.Service
{
    public interface IIntegrationService
    {
        Task<string> GetNexudusUrlAsync(string type);

        Task<string> GetRoomzillaUrlAsync();
    }
}