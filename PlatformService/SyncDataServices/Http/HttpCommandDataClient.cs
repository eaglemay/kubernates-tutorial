using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
  public class HttpCommandDataClient : ICommandDataClient
  {
    private readonly HttpClient _httpClient;
    public IConfiguration _configuration { get; }

    public HttpCommandDataClient(HttpClient HttpClient, IConfiguration configuration)
    {
        _httpClient = HttpClient;
        _configuration = configuration;
    }
    public async Task SendPlatformToCommand(PlatformReadDto plat)
    {
            var httpContent = new StringContent( 
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
            }
    }
  }  
}