using System.ComponentModel.DataAnnotations;

namespace Inventra.Models.Categories
{
    public class CategoryCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
