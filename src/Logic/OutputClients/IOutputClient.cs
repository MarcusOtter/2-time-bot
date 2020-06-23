using System.Threading.Tasks;

namespace Logic.OutputClients
{
    public interface IOutputClient
    {
        Task<bool> SendMessageAsync(TwoTimeMessage text);
    }
}
