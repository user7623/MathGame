using System.Threading.Tasks;

namespace MathGame.Services.Interfaces
{
    public interface IActivePlayersService
    {
        Task<int> GetActivePlayersInRoom();
    }
}
