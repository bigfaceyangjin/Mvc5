using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreightSynchro
{
    public abstract class BaseEntity<T, TID> where T : BaseEntity<T, TID>
    {
        public virtual TID ID { get; set; }

        #region
        /// <summary>
        /// Session配置文件路径
        /// </summary>
        protected static readonly string SessionFactoryConfigPath = System.IO.Path.Combine(Application.StartupPath, "NHibernate.config");

        /// <summary>
        /// 返回对应的Session.
        /// </summary>
        protected static ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath);
            }
        }
        #endregion

        #region common
        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        public static T GetById(TID id, bool shouldLock)
        {
            T entity;

            if (shouldLock)
            {
                entity = NHibernateSession.Get<T>(id, LockMode.Upgrade);
            }
            else
            {
                entity = NHibernateSession.Get<T>(id);
            }

            return entity;
        }

        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        public static T GetById(TID id)
        {
            return GetById(id, false);
        }

        /// <summary>
        /// 获取所有的类型为T的对象
        /// </summary>
        public static IList<T> GetAll()
        {
            return GetByCriteria();
        }

        /// <summary>
        /// 根据给定的 <see cref="ICriterion" /> 来查询结果
        /// 如果没有传入 <see cref="ICriterion" />, 效果与 <see cref="GetAll" />一致.
        /// </summary>
        public static IList<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(typeof(T));

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }
        #endregion

        #region entity
        /// <summary>
        /// 根据exampleInstance的属性值来查找对象，返回与其值一样的对象对表。
        /// exampleInstance中值为0或NULL的属性将不做为查找条件
        /// </summary>
        /// <param name="exampleInstance">参考对象</param>
        /// <param name="propertiesToExclude">要排除的查询条件属性名</param>
        /// <returns></returns>
        public virtual IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(exampleInstance.GetType());
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }
            example.ExcludeNone();
            example.ExcludeNulls();
            example.ExcludeZeroes();
            criteria.Add(example);
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }

        /// <summary>
        /// 使用<see cref="GetByExample"/>来返回一个唯一的结果，如果结果不唯一会抛出异常
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public virtual T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            IList<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0)
            {
                return foundList[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将指定的对象保存到数据库，并立限提交，并返回更新后的ID
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public virtual T Save()
        {
            T entity = (T)this;
            NHibernateSession.Save(entity);
            NHibernateSession.Flush();
            return entity;
        }



        /// <summary>
        /// 将指定的对象保存或更新到数据库，并返回更新后的ID
        /// </summary>
        public virtual T Merge()
        {
            T entity = (T)this;
            NHibernateSession.Merge<T>(entity);
            NHibernateSession.Flush();
            return entity;
        }

        ///// <summary>
        ///// 从数据库中删除指定的对象
        ///// </summary>
        //public virtual void Delete()
        //{
        //    T entity = (T)this;
        //    NHibernateSession.Delete(entity);
        //    NHibernateSession.Flush();
        //}

        public virtual DbTransaction BeginTransaction()
        {
            ITransaction tran = NHibernateSession.BeginTransaction();// NHibernateSessionManager.Instance.BeginTransactionOn(SessionFactoryConfigPath);
            return new DbTransaction(tran);
        }

        /// <summary>
        /// 提交所有的事务对象，并Flush到数据库
        /// </summary>
        public virtual void CommitChanges()
        {
            if (NHibernateSessionManager.Instance.HasOpenTransactionOn(SessionFactoryConfigPath))
            {
                NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
            }
            else
            {
                // 如果不是事务模式，就直接调用Flush来更新                
                NHibernateSession.Flush();
            }
        }
        #endregion

        #region WebApi获取数据
        public static string Url
        {
            get
            {
                string url = System.Configuration.ConfigurationManager.AppSettings[typeof(T).Name];
                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception(string.Format("“{0}”未包含URL配置", typeof(T).Name));
                }
                return url;
            }
        }

        public static List<T> GetAllBySource()
        {
            return WebApiClient<T>.GetAll(Url);
        }

        public static void EditBySource(List<int> value)
        {
            WebApiClient<T>.Edit(Url, value);
        }

        public static T GetOneBySource(string id)
        {
            return WebApiClient<T>.Get(Url, id);
        }
        #endregion
    }

    public class DbTransaction : IDisposable
    {
        ITransaction _transaction;

        public DbTransaction(ITransaction transaction)
        {

            _transaction = transaction;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        #endregion

        #region ITransaction 成员

        public void Begin(System.Data.IsolationLevel isolationLevel)
        {
            _transaction.Begin(isolationLevel);
        }

        public void Begin()
        {
            _transaction.Begin();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Enlist(System.Data.IDbCommand command)
        {
            _transaction.Enlist(command);
        }

        public bool IsActive
        {
            get { return _transaction.IsActive; }
        }

        public void RegisterSynchronization(NHibernate.Transaction.ISynchronization synchronization)
        {
            _transaction.RegisterSynchronization(synchronization);
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public bool WasCommitted
        {
            get { return _transaction.WasCommitted; }
        }

        public bool WasRolledBack
        {
            get { return _transaction.WasRolledBack; }
        }

        #endregion
    }
}
