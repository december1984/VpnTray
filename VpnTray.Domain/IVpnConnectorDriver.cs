using System.Threading.Tasks;

namespace VpnTray.Domain
{
    public interface IVpnConnectorDriver
    {
        Task Connect(string id);
        Task Disconnect(string id);
    }
}