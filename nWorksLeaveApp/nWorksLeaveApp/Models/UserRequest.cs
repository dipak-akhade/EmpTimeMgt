using System;

namespace nWorksLeaveApp
{
	public class UserRequest
	{
		public string leavedate { get; set; }
		public string requestedAs { get; set; }
		public string isWithPay{ get; set;}
		public string isAccepted{ get; set;}
	}
}