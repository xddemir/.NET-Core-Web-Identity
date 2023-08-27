using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.IdentityExamaple.Pages;

[Authorize(Policy="MustBelongToHrDepartment")]
public class HumanResources : PageModel
{
    public void OnGet()
    {
        
    }
}