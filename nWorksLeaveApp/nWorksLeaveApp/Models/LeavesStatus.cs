using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class LeavesStatus
	{
		public int totalLeaves { get; set; }
		public int totalHolidays { get; set; }
		public float balanceLeaves { get; set; }
		public float balanceHolidays { get; set; }
		public List<Leaves> leaves { get; set; }
		public List<Holidays> holidays { get; set; }

	}
	public class Holidays
	{
		public string date { get; set; }
		public string weekday { get; set; }
	}

	public class Leaves
	{
		public string date { get; set; }
		public string weekday { get; set; }
		public string leaveType{ get; set;}
	}
}

