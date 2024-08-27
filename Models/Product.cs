using System.ComponentModel.DataAnnotations;

namespace NetProductApp.Models
{
public class Product{
    [Key]
    public int id{set; get;}
    [Required]
    public string name{set; get;}
    [Required]
    public decimal price{set; get;}
    public string image_path{set; get;}
}
}