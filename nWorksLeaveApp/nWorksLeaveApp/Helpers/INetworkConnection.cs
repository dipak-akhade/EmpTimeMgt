using System;

namespace nWorksLeaveApp
{
	public interface INetworkConnection
	{
		bool IsConnected { get; }
		void CheckNetworkConnection();
	}
}

