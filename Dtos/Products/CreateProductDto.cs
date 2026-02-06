using System.ComponentModel.DataAnnotations;

namespace Webshop.Dtos.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Der Name ist erforderlich.")]
        [StringLength(100, ErrorMessage = "Der Name darf maximal 100 Zeichen lang sein.")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Die Beschreibung darf maximal 500 Zeichen lang sein.")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Der Basispreis ist erforderlich.")]
        [Range(0.01, 999999.99, ErrorMessage = "Der Basispreis muss zwischen 0,01 und 999.999,99 liegen.")]
        public decimal BasePrice { get; set; }
        
        [Required(ErrorMessage = "Die Kategorie ist erforderlich.")]
        public int CategoryId { get; set; }
    }
}
