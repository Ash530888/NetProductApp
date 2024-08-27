using System.ComponentModel.DataAnnotations;

namespace NetProductApp.Models
{
public class Login{
    [Key]
    public string username{set; get;}
    [Required]
    public string password{set; get;}
    [Required]
    public string role{set; get;}
}
}