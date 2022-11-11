using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using ProjectBase.Utils;
using System.Reflection;
using NHibernate.Mapping.ByCode;
using System.Threading;


namespace ProjectBase.Data
{
    /// <summary>
    /// �ṩһ�������͹��� Session�������ͳһ�ӿ�
    /// </summary>
    public sealed class NHibernateSessionManager
    {
        #region ����һ���̰߳�ȫ�ĵ���

        /// <summary>
        /// ����һ���̰߳�ȫ�ĵ���
        /// </summary>
        public static NHibernateSessionManager Instance {
            get {
                return Nested.NHibernateSessionManager;
            }
        }

        private NHibernateSessionManager() { }

        /// <summary>
        /// ����һ���̰߳�ȫ�ĵ���
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }

        #endregion

        bool IsAutoFlush = false;

        /// <summary>
        /// ���� <see cref="sessionFactories" /> ����һ��session factory�����û���ҵ��򴴽�һ���µ�
        /// </summary>
        /// <param name="sessionFactoryConfigPath">�����ļ�·��</param>
        private ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath) {
            Check.Require(string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  ��hashTable����ȡ��һ���Ѿ����ڵ�ISessionFactory
            ISessionFactory sessionFactory = (ISessionFactory) sessionFactories[sessionFactoryConfigPath];

            //  ���û��ƥ��� SessionFactory �򴴽�һ���µ�
            if (sessionFactory == null) {
                Check.Require(!File.Exists(sessionFactoryConfigPath),
                    "The config file at '" + sessionFactoryConfigPath + "' could not be found");
                
                NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
                Configuration cfg = new Configuration();
                cfg.Configure(sessionFactoryConfigPath);
                
                var data = System.Configuration.ConfigurationManager.AppSettings["Data"];
                if (!string.IsNullOrEmpty(data))
                {
                    ModelMapper mapper = new ModelMapper();
                    foreach (var assembly in data.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        MapModel(assembly, mapper);
                    }
                    var hbmMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    cfg.AddMapping(hbmMapping);
                }
                
                sessionFactory = cfg.BuildSessionFactory();


                if (sessionFactory == null)
                {
                    throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                sessionFactories[sessionFactoryConfigPath]= sessionFactory;
            }

            return sessionFactory;
        }

        /// <summary>
        /// �ӳ����м�ӳ���ൽNhibernate
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapper"></param>
        private static void MapModel(string data, ModelMapper mapper)
        {
            var assembly = Assembly.Load(data);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterface("IEntitySqlsMapper") != null)
                {
                    mapper.AddMapping(type);
                }
            }
        }


        /// <summary>
        /// ע���ƽ�����û�д򿪹���Session���棬������������IHttpModule�������
        /// </summary>
        public void RegisterInterceptorOn(string sessionFactoryConfigPath, IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSessionFrom(sessionFactoryConfigPath, interceptor);
        }

        /// <summary>
        /// ���� <see cref="sessionFactories" /> ����һ��session�����û���ҵ��򴴽�һ���µ�
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        public ISession GetSessionFrom(string sessionFactoryConfigPath) {
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        /// <summary>
        /// ���� <see cref="sessionFactories" /> ����һ��session����ע��ָ���������������û���ҵ��򴴽�һ���µ�
        /// </summary>
        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];
            
            if (session == null) {
                if (interceptor != null) {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }
                session.FlushMode = FlushMode.Never;
                ContextSessions[sessionFactoryConfigPath] = session;
            }
            
            Check.Ensure(session == null, "session was null");            
            return session;
        }

        /// <summary>
        /// Flushָ����Session��ر����ݿ����ӡ�
        /// </summary>
        public void CloseSessionOn(string sessionFactoryConfigPath) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                if (IsAutoFlush)
                    session.Flush();
                session.Close();
            }

            ContextSessions.Remove(sessionFactoryConfigPath);
        }

        /// <summary>
        /// Flush�͹ر����е�Session
        /// </summary>
        public void CloseAllSession()
        {
            System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
            foreach (string item in ContextSessions.Keys)
                keys.Add(item);
            foreach (string item in keys)
                CloseSessionOn(item);
        }

        
        public ITransaction BeginTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            if (transaction == null) {
                transaction = GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();
                ContextTransactions.Add(sessionFactoryConfigPath, transaction);
            }

            return transaction;
        }

        public void CommitTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try {
                if (HasOpenTransactionOn(sessionFactoryConfigPath)) {
                    transaction.Commit();
                    ContextTransactions.Remove(sessionFactoryConfigPath);
                }
            }
            catch (HibernateException) {
                RollbackTransactionOn(sessionFactoryConfigPath);
                throw;
            }
        }

        public bool HasOpenTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try {
                if (HasOpenTransactionOn(sessionFactoryConfigPath)) {
                    transaction.Rollback();
                }

                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
            finally {
                CloseSessionOn(sessionFactoryConfigPath);
            }
        }

        private static Hashtable _ContextTransactions = new Hashtable();

        /// <summary>
        /// ���������������񼯺ϣ�һ��requestһ�����񼯺�
        /// </summary>
        private Hashtable ContextTransactions {
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[TRANSACTION_KEY] == null)
                        HttpContext.Current.Items[TRANSACTION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                return new Hashtable();
            }
        }

        /// <summary>
        /// ������������Session���ϣ�һ��requestһ��Session���ϣ���ͬ�����ݿ�Session�ɷ��ڴ˼�����
        /// </summary>
        private Hashtable ContextSessions {            
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[SESSION_KEY] == null)
                        HttpContext.Current.Items[SESSION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[SESSION_KEY];
                }
                return _ContextTransactions;
            }
        }

        private bool IsInWebContext() {
            return HttpContext.Current != null;
        }

        private Hashtable sessionFactories = new Hashtable();
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTIONS";
        private const string SESSION_KEY = "CONTEXT_SESSIONS";
        private const string ANALYZE_PROPERTIES = "connection.connection_string";
        
    }
}
