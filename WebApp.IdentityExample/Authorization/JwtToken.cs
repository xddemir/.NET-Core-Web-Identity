using Newtonsoft.Json;

namespace WebApp.IdentityExamaple.Authorization;

public class JwtToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("ex")]
    public DateTime ExpiresAt { get; set; }
}