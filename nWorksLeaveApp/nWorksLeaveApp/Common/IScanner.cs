using System;
using System.Threading.Tasks;

namespace nWorksLeaveApp
{
	public interface IScanner
	{	
		Task<string> Scan();
	}
}

