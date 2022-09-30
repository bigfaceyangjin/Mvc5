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
	public class LogBeforeBehavior : IInterceptionBehavior
	{
		Logger loger = new Logger(typeof(LogBeforeBehavior));
		public bool WillExecute { get { return true; } }

		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
		{
			//方法执行前的全部动作
			loger.Info($"{this.GetType().Name}_LogBeforeBehavior");
			return getNext()(input,getNext);
		}
	}
}
