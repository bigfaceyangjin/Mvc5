using Eonup.Framework.Log4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;
namespace Eonup.Framework.AOP
{
	/// <summary>
	/// 方法执行前的日志写入
	/// </summary>
	public class LogAfterBehavior : IInterceptionBehavior
	{
		Logger loger = new Logger(typeof(LogBeforeBehavior));
		public bool WillExecute { get { return true; } }

		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
		{
			IMethodReturn method= getNext()(input, getNext);
			loger.Info($"{this.GetType().Name}_LogAfterBehavior");
			return method;
		}
	}
}
