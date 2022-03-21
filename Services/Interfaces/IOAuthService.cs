using System.Threading.Tasks;

namespace forgeSample.Services.Interfaces
{
    public interface IOAuthService
    {
        Task<dynamic> GetPublicAsync();
        Task<dynamic> GetInternalAsync();
        string GetSettings(string setting);
    }
}