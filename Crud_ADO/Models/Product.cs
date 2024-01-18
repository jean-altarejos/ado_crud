using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Crud_ADO.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Qty { get; set; }
        public string Remarks { get; set; }
    }
}
