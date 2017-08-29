using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class inoutDetailsPerDate
	{
		public  List<inTimes> inTimes { get; set; }
		public  List<outTimes> outTimes { get; set; }
	}
	public class inTimes
	{
		public string INTIMES { get; set; }
		public string deviceid { get; set; }
		public string location { get; set; }
		public string IsDevice_or_LocationChanged { get; set; }//
	}
	public class outTimes
	{
		public string OUTTIMES { get; set; }
		public string deviceid { get; set; }
		public string location { get; set; }
		public string IsDevice_or_LocationChanged { get; set; }//
	}

}

