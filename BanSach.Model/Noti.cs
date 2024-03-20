using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.Models
{
    public class Noti
    {
        [Key]
        public int NotiId { get; set; } = 0;
        public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
        public int FromUserId { get; set; } = 0;
        public int ToUserId { get; set; } = 0;
        public string NotiHeader { get; set; } = "";
        public string NotiBody { get; set; } = "";
        public bool IsRead { get; set; } = false;
        public string Url { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Message { get; set; } = "";

        public string CreatedDateSt => this.CreatedDate.ToString("dd-MMM-yyyy HH:mm:ss");
        public string IsReadSt => this.IsRead ? "YES" : "NO";

        public string FromUserName { get; set; } = "";
        public string ToUserName { get; set; } = "";
    }
}
