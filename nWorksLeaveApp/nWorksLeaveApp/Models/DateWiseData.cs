using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class DateWiseData
	{
		public string _Date { get; set; }
		public List<DateData> dateData { get; set; }
	}
	public class DateData
	{
		public string uid { get; set; }
		public string employeeName { get; set; }
		public string _date { get; set; }
		public string fin { get; set; }
		public string fout { get; set; }
		public string weekday { get; set; }
		public string totalInTime { get; set; }
		public string Is_Loc_Device_Changed { get; set; }
	}
}

