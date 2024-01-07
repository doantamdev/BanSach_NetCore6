using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bansach.Utility
{
    public static class SD
    {
        public const string Role_User_Indi = "Individual";
        public const string Role_User_Comp = "Company";
        public const string Role_User_Admin = "Admin";
        public const string Role_User_Employee = "Employee";


		public const string StatusPending = "Pending"; //trạng thái ban đầu
		public const string StatusApproved = "Approved"; //phê duyệt
		public const string StatusInProcess = "Processing"; //quá trình
		public const string StatusShipped = "Shipped";	//giao
		public const string StatusCancelled = "Cancelled";	//hủy
		public const string StatusRefunded = "Refunded";	//hoàn

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment"; //công ty thì thanh toán chậm
		public const string SessionCart = "SessionShoppingCart"; //30 ngày thanh toán
	}
}
