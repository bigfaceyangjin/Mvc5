using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eonup.Wcf.ConsoleService
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				WcfInit.OpenService();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{DateTime.Now}，线程Id：{Thread.CurrentThread.ManagedThreadId} Msg:{ex.Message}");
			}
		}
	}
}
