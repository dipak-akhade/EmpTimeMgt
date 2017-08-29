using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class ModelGetEmployeeList
	{
			public string ServiceStatus { get; set; }
			public List<EmployeeList> EmpList { get; set; }
	}
	public class EmployeeList
	{
		public string uid { get; set; }
		public string fname { get; set; }
		public string lname { get; set; }
	}
}

