using System.ComponentModel.DataAnnotations;

namespace Webshop.Dtos.Categories
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Der Name ist erforderlich.")]
        [StringLength(100, ErrorMessage = "Der Name darf maximal 100 Zeichen lang sein.")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Die Beschreibung darf maximal 500 Zeichen lang sein.")]
        public string? Description { get; set; }
    }
}
