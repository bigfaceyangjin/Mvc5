using System;
using System.Collections.Generic;
using NHibernate;
using ProjectBase.Utils;
using NHibernate.Criterion;
using System.Configuration;
using ProjectBase.Utils.Entities;

namespace ProjectBase.Data
{
    /// <summary>
    /// ���ݳ־û�����
    /// </summary>
    /// <typeparam name="T">Ҫ�־û�����������</typeparam>
    /// <typeparam name="TID">ID�ֶε���������</typeparam>
    public abstract class AbstractNHibernateDao<T, TId> : IDao<T, TId>
    {
        private const string APPSETTING_NAME = "sessionPath";
                
        /// <param name="sessionFactoryConfigPath">ָ��Session�����������ļ�</param>
        protected AbstractNHibernateDao(string sessionFactoryConfigPath) {
            if (string.IsNullOrEmpty(sessionFactoryConfigPath))
            {
                sessionFactoryConfigPath = "NHibernate.config";
            }
            //���·���а��� :(�̷�) ���ţ���֤���������һ������·������ֱ��ʹ�á�
            //����������·������WEB�����У���Server.MapPathת���ɾ���·������App�����У�����ϵ�ǰ����Ŀ¼
            if (sessionFactoryConfigPath.Contains(":"))
                SessionFactoryConfigPath = sessionFactoryConfigPath;
            else if (System.Web.HttpContext.Current != null)
                SessionFactoryConfigPath = System.Web.HttpContext.Current.Server.MapPath("~/"+sessionFactoryConfigPath);
            else
                SessionFactoryConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sessionFactoryConfigPath);
        }

        /// <summary>
        /// ��ʼ��AbstractNHibernateDao�࣬Ĭ��ʹ��config�ļ�Appsetting�� sessionPath��ָ�����ļ����г�ʼ��
        /// </summary>
        protected AbstractNHibernateDao()
            : this(ConfigurationManager.AppSettings[APPSETTING_NAME])
        {
        }

        /// <summary>
        /// ����ID�����ݿ��ȡһ������ΪT��ʵ��
        /// </summary>
        public T GetById(TId id, bool shouldLock)
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
        /// ����ID�����ݿ��ȡһ������ΪT��ʵ��
        /// </summary>
        public T GetById(TId id)
        {
            return GetById(id, false);
        }

        /// <summary>
        /// ��ȡ���е�����ΪT�Ķ���
        /// </summary>
        public IList<T> GetAll() {
            return GetByCriteria();
        }

        /// <summary>
        /// ���ݸ����� <see cref="ICriterion" /> ����ѯ���
        /// ���û�д��� <see cref="ICriterion" />, Ч���� <see cref="GetAll" />һ��.
        /// </summary>
        public IList<T> GetByCriteria(params ICriterion[] criterion) {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            foreach (ICriterion criterium in criterion) {
                criteria.Add(criterium);
            }            
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }

        /// <summary>
        /// ����exampleInstance������ֵ�����Ҷ��󣬷�������ֵһ���Ķ���Ա�
        /// exampleInstance��ֵΪ0��NULL�����Խ�����Ϊ��������
        /// </summary>
        /// <param name="exampleInstance">�ο�����</param>
        /// <param name="propertiesToExclude">Ҫ�ų��Ĳ�ѯ����������</param>
        /// <returns></returns>
        public IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(exampleInstance.GetType());
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude) {
                example.ExcludeProperty(propertyToExclude);
            }
            example.ExcludeNone();
            example.ExcludeNulls();
            example.ExcludeZeroes();
            criteria.Add(example);
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }

        public IPageOfList<T> GetByExample(T exampleInstance, int pageIndex, int pageSize, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude) {
                example.ExcludeProperty(propertyToExclude);
            }
            example.ExcludeNone();
            example.ExcludeNulls();
            example.ExcludeZeroes();
            
            int recordTotal = Convert.ToInt32((criteria.Clone() as ICriteria).SetProjection(Projections.Count("ID")).UniqueResult());

            criteria.AddOrder(new Order("ID", false));
            criteria.Add(example).SetFirstResult(pageIndex*pageSize).SetMaxResults(pageSize);

            return new PageOfList<T>(criteria.List<T>(), pageIndex, pageSize, recordTotal);
        }

        /// <summary>
        /// ʹ��<see cref="GetByExample"/>������һ��Ψһ�Ľ������������Ψһ���׳��쳣
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude) {
            IList<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1) {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0) {
                return foundList[0];
            }
            else {
                return default(T);
            }
        }

        /// <summary>
        /// ��ָ���Ķ��󱣴浽���ݿ⣬�������ύ�������ظ��º��ID
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public T Save(T entity) {
            NHibernateSession.Save(entity);
            NHibernateSession.Flush();
            return entity;
        }

        /// <summary>
        /// ��ָ���Ķ��󱣴����µ����ݿ⣬�����ظ��º��ID
        /// </summary>
        public T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            NHibernateSession.Flush();
            return entity;
        }

        /// <summary>
        /// �����ݿ���ɾ��ָ���Ķ���
        /// </summary>
        public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
            NHibernateSession.Flush();
        }

        public DbTransaction BeginTransaction()
        {
            ITransaction tran = NHibernateSession.BeginTransaction();// NHibernateSessionManager.Instance.BeginTransactionOn(SessionFactoryConfigPath);
            return new DbTransaction(tran);
        }

        /// <summary>
        /// �ύ���е�������󣬲�Flush�����ݿ�
        /// </summary>
        public void CommitChanges() {
            if (NHibernateSessionManager.Instance.HasOpenTransactionOn(SessionFactoryConfigPath)) {
                NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
            }
            else {
                // �����������ģʽ����ֱ�ӵ���Flush������                
                NHibernateSession.Flush();
            }
        }

        /// <summary>
        /// ���ض�Ӧ��Session.
        /// </summary>
        protected ISession NHibernateSession {
            get {
                return NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private Type persitentType = typeof(T);
        
        /// <summary>
        /// Session�����ļ�·��
        /// </summary>
        protected readonly string SessionFactoryConfigPath;

        public virtual IPageOfList<T> GetByFilter(ParameterFilter filter)
        {
            string sql = " from " + typeof(T).Name + " a where 1=1 ";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateQuery("select count(*) " + sql);
            var query = NHibernateSession.CreateQuery("select a " + sql + filter.GetOrderString());

            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }

            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            long recodTotal = Convert.ToInt64(countQuery.UniqueResult());
            var list = query.SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<T>();
            return new PageOfList<T>(list, pageIndex, pageSize, recodTotal);
        }
    }
}
