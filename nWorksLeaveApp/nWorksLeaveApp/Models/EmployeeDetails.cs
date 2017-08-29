using System;
using System.Collections.Generic;

namespace nWorksLeaveApp.Models
{
	public class EmployeeDetails
	{
		public string ServiceStatus { get; set; }
		public Employee Emp { get; set; }       
	}

	public class Employee
	{
		public string uid { get; set; }
		public string fname { get; set; }
		public string mname { get; set; }
		public string lname { get; set; }
		public DateTime dob { get; set; }
		public string gender { get; set; }
		public string desg { get; set; }
		public DateTime hire_date { get; set; }
		public string username { get; set; }
		public string addressLine1 { get; set; }
		public string addressLine2 { get; set; }
		public string state { get; set; }
		public string city { get; set; }
		public string pincode { get; set; }
		public string country { get; set; }
		public string mobileNo { get; set; }
		public DateTime leaveDate { get; set; }
		public DateTime anniversary { get; set; }
		public string email { get; set; }
		public string userActive { get; set; }
		public float Additional_Earned_Leaves { get; set; }
		public float Leaves_Rolled_Over_from_Previous_yr { get; set; }
	}
}

