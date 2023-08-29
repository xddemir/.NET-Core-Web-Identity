using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.IdentityExample.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    [HttpPost]
    public IActionResult Authenticate([FromBody]Credential credential)
    {
        if (credential.UserName == "admin" && credential.Password == "admin")
        {
            // creating the security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@admin.com"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2023-04-27")
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            return Ok(new
            {
                access_token = CreateToken(claims, expiresAt),
                expires_at = expiresAt
            });
        }
        
        ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint");
        return Unauthorized(ModelState);
    }

    private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
    {
        var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey")?? "");
        
        var jwt = new JwtSecurityToken(claims: claims, 
            notBefore: DateTime.UtcNow,
            expires: expireAt, 
            signingCredentials:new SigningCredentials(
                new SymmetricSecurityKey(secretKey), 
                SecurityAlgorithms.HmacSha256Signature
                ));
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}

public class Credential
{
    public string UserName { get; set; }
    public string Password { get; set; }
}