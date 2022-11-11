using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using ProjectBase.Utils;
using System.Threading;
using System.Reflection;
using ProjectBase.Data;

namespace ProjectBase.Data.NHibernateSessionMgmt
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LogInterceptor : NHibernate.EmptyInterceptor
    {
        #region IInterceptor 成员

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {            
            LogManager.Logger.Info(new LogMessage(UserId, SaveAction.Insert, entity.GetType(),null, id));
            //RaiseDomainEvent(entity, SaveAction.Insert);
            return true;
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            LogManager.Logger.Info(new LogMessage(UserId, SaveAction.Delete, entity.GetType(), null, id));
            //RaiseDomainEvent(entity, SaveAction.Delete);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            LogManager.Logger.Info(new LogMessage(UserId, SaveAction.Update, entity.GetType(), null, id));
            //RaiseDomainEvent(entity, SaveAction.Update);
            return true;
        }

        private void RaiseDomainEvent(object entity, SaveAction action)
        {
            DomainObjectBase<int> domain = entity as DomainObjectBase<int>;
            if (domain == null)
                return;
            var mi= entity.GetType().GetMethod("OnSaved", BindingFlags.Static|BindingFlags.Public);
            domain.GetType().InvokeMember("OnSaved", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[] { domain, action });
            //mi.Invoke(
            //DomainObjectBase<int>.OnSaved(domain, action);
        }
        #endregion


        private object UserId
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }

    }
}
