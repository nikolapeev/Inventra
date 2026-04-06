using System.ComponentModel.DataAnnotations;

namespace Inventra.Core.ViewModels.Categories
{
    public class CategoryCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
