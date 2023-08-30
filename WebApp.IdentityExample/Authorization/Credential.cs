using System.ComponentModel.DataAnnotations;

namespace WebApp.IdentityExamaple.Authorization;

public class Credential
{
    [Required] [Display(Description = "User Name")]
    public string Name { get; set; }
    [Required] [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}