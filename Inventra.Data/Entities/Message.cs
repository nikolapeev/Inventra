using Inventra.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data.Entities
{
    public class Message
    {
        [Key]
        public Guid Id {  get; set; }

        [Required]
        public string CreatedBy {  get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        [Required]
        [MinLength(10,ErrorMessage ="Message must be between 10 and 50 characters long")]
        [MaxLength(50, ErrorMessage = "Message must be between 10 and 50 characters long")]
        public string Content { get; set; } = null!;

        [Required]
        public MessageType Type { get; set; } 
    }
}
