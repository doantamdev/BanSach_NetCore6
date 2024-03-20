using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.Models
{
    [Table("BotChat")]
    public class Chat
    {
        [Column("Id")]
        [Display(Name = "ChatId")]
        public int Id { get; set; }

        [Column("Reply")]
        [Display(Name = "Reply")]
        public string Reply { get; set; }

        [Column("Message")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
