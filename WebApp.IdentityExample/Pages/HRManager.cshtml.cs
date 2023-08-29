using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.IdentityExamaple.DTOs;

namespace WebApp.IdentityExamaple.Pages;

[Authorize(Policy="HRManagerOnly")]
public class HRManager : PageModel
{
    private readonly IHttpClientFactory _client;

    [BindProperty] public List<WeatherForecastDTO> weatherForecastItems { get; set; } = new List<WeatherForecastDTO>();
    
    public HRManager(IHttpClientFactory client)
    {
        _client = client;
    }
    
    public async Task OnGetAsync()
    {
        var httpClient = _client.CreateClient("MyWebApi");
        weatherForecastItems =  await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
    }
}