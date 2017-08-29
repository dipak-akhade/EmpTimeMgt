using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class EmployeeWiseData
	{
		public string EmployeeName { get; set; }
		public List<EmployeeData> employeeWiseData { get; set; }
	}
	public class EmployeeData
	{
		public string uid { get; set; }
		public string In { get; set; }
		public string Out { get; set; }
		public string date { get; set; }
		public string weekday { get; set; }
		public string totalInTime { get; set; }
		public string Is_Loc_Device_Changed { get; set; }

	}
}

