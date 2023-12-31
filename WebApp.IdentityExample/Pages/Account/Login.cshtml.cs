using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.IdentityExamaple.Authorization;

namespace WebApp.IdentityExamaple.Pages.Account;

public class Login : PageModel
{
    [BindProperty]
    public Credential Credential { get; set; } = new Credential();
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // verify credentials
        if (Credential.Name == "admin" && Credential.Password == "admin")
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

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = Credential.RememberMe
            };
            
            await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties);

            return RedirectToPage("/Index");
        }

        return Page();
    }
}

