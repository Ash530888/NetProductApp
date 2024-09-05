using System.ComponentModel.DataAnnotations;

namespace NetProductApp.Models{
    public class Stock{
        [Key]
        public int id{set; get;}
        [Required]
        public int num_instock{set; get;}
    }
}