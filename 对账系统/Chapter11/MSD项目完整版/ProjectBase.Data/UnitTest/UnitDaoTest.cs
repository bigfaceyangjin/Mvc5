using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Testing;
using System.Collections;
using NHibernate;
using System.Configuration;
using Microsoft.Practices.Unity;

namespace ProjectBase.Data.UnitTest
{
    public abstract class UnitDaoTest
    {
        private const string APPSETTING_NAME = "sessionPath";

        public UnitDaoTest()
        {
            RegisterContainer(IocContainer.Instance.Container);
        }

        protected abstract void RegisterContainer(UnityContainer container);

        protected ISession Session
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSessionFrom(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[APPSETTING_NAME]));
            }
        }

        /// <summary>
        /// 返回一个类的Map验证器
        /// </summary>
        /// <typeparam name="T">需要验证的类</typeparam>
        /// <returns></returns>
        protected PersistenceSpecification<T> GetVerifier<T>()
        {
            return new PersistenceSpecification<T>(Session);
        }

        protected PersistenceSpecification<T> GetVerifier<T>(IEqualityComparer ec)
        {
            return new PersistenceSpecification<T>(Session, ec);
        }

        public class CustomEqualityComparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x is DateTime && y is DateTime)
                {
                    return Convert.ToDateTime(x).ToString() == Convert.ToDateTime(y).ToString();
                }
                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        public void Close()
        {
            NHibernateSessionManager.Instance.CloseAllSession();
        }
    }
}
