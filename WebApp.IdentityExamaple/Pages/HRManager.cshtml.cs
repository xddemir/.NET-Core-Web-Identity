using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.IdentityExamaple.Pages;

[Authorize(Policy="HRManagerOnly")]
public class HRManager : PageModel
{
    public void OnGet()
    {
        
    }
}