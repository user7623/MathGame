using MathGame.Services.Interfaces;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MathGame.Services
{
    public class ActivePlayersService : IActivePlayersService
    {
        public readonly HttpClient _httpClient;

        public ActivePlayersService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<int> GetActivePlayersInRoom()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:44386/Game/GetActivePlayers");

                if (response.IsSuccessStatusCode)
                {
                    var activePlayers = int.Parse(await response.Content.ReadAsStringAsync());
                    return activePlayers;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve active players. Status code: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                Log.Error("Exception in ActivePlatersService -> GetActivePlayersInRoom");
                Log.Error(ex.ToString());
                return -1;
            }
        }
    }
}
