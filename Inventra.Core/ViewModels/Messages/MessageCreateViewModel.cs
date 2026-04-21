using Inventra.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.ViewModels.Messages
{
    public class MessageCreateViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Message must be between 10 and 50 characters long")]
        [MaxLength(50, ErrorMessage = "Message must be between 10 and 50 characters long")]
        public string Content { get; set; } = null!;

        [Required]
        public MessageType Type { get; set; }
    }
}
