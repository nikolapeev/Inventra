using Inventra.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.ViewModels.Messages
{
    public class MessageIndexViewModel
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Content { get; set; } = null!;

        public MessageType Type { get; set; }
    }
}
