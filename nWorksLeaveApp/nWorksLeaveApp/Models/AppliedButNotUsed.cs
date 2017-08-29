using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class AppliedButNotUsed
	{
		public List<Leaves> leaves { get; set; }
		public List<Holidays> holidays { get; set; }
	}


	public class H
	{
		public string date { get; set; }
		public string weekday { get; set; }
	}

	public class L
	{
		public string date { get; set; }
		public string weekday { get; set; }
		public string leaveType { get; set; }
	}
}

