using System;
using System.Collections.Generic;

namespace nWorksLeaveApp
{
	public class model_ConfirmLeaves
	{
		public string userId { get; set; }
		public List<model_Dates> Dates { get; set;}
		public List<model_DeselectedDates> deselectedDates { get; set;}
	}

	public class model_Dates
	{
		public string selectedDate { get; set; }
		public string requestedAs { get; set; }
		public string isWithPay{ get; set; }
	}
	public class model_DeselectedDates
	{
		public string selectedDate { get; set; }
		public string requestedAs { get; set; }
	}
}

