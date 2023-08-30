using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.IdentityExamaple.Authorization;
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
        // get token from session
        JwtToken token = new JwtToken();

        var strToken = HttpContext.Session.GetString("access_token");

        if (string.IsNullOrEmpty(strToken))
        {
            token = await Authenticate();
        }
        else
        {
            token = JsonConvert.DeserializeObject<JwtToken>(strToken) ?? new JwtToken();
        }

        if (token == null || 
            string.IsNullOrWhiteSpace(token.AccessToken) || 
            token.ExpiresAt <= DateTime.Now) {
            token = await Authenticate();
        }
        
        var httpClient = _client.CreateClient("MyWebApi");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
        weatherForecastItems =  await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
    }

    private async Task<JwtToken> Authenticate()
    {
        var httpClient = _client.CreateClient("MyWebApi");
        var res = await httpClient.PostAsJsonAsync("Auth", new Credential
        {
            Name = "admin",
            Password = "admin"
        });

        res.EnsureSuccessStatusCode();
        string jwt = await res.Content.ReadAsStringAsync();
        
        HttpContext.Session.SetString("access_token", jwt);
        
        return JsonConvert.DeserializeObject<JwtToken>(jwt) ?? new JwtToken();
    
    }
}