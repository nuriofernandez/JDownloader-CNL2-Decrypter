using System.ServiceProcess;
using System.Threading;

namespace JDownloaderService
{
	static class Program
	{

		static void Main()
		{
#if DEBUG
				// While debugging this section is used.
				Service service = new Service();
				service.onDebug();
				Thread.Sleep(Timeout.Infinite);
#else
			ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new Service()
				};
				ServiceBase.Run(ServicesToRun);
			#endif
		}

	}
}
