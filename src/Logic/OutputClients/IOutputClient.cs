using System.Threading.Tasks;

namespace Logic.OutputClients
{
    public interface IOutputClient
    {
        Task<bool> SendMessage(TwoTimeMessage text);
    }
}
