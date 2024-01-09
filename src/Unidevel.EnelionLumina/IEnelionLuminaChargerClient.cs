
namespace Unidevel.EnelionLumina
{
    public interface IEnelionLuminaChargerClient
    {
        Task LoginAsync(string hostname, string userName, string password);
        Task LogoutAsync();
        Task SetMainsAsync(Phases phasesLimit, int currentLimitAmp);
    }
}