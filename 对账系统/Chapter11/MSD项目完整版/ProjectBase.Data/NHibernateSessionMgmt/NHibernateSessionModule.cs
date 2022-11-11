using System;
using System.Configuration;
using System.Web;
using ProjectBase.Utils;
using ProjectBase.Data.NHibernateSessionMgmt;

namespace ProjectBase.Data
{
    /// <summary>
    /// ʹ�� <see cref="NHibernateSessionManager" /> ʵ�� Open-Session-In-View ģ��
    /// </summary>
    public class NHibernateSessionModule : IHttpModule
    {
        public void Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        /// <summary>
        /// ����������ʼǰ������
        /// </summary>
        /// <param name="sender"></param>
        private void BeginTransaction(object sender, EventArgs e) {
            string path= System.Web.HttpContext.Current.Server.MapPath("~/"+ConfigurationManager.AppSettings["sessionPath"]);
            NHibernateSessionManager.Instance.RegisterInterceptorOn(path, new LogInterceptor());
            //OpenSessionInViewSection openSessionInViewSection = GetOpenSessionInViewSection();

            //foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories) {
            //    if (sessionFactorySettings.IsTransactional) {
            //        NHibernateSessionManager.Instance.BeginTransactionOn(sessionFactorySettings.FactoryConfigPath);
            //    }
            //}
        }

        /// <summary>
        /// ���������ʱ�Զ��ύ���ر����д򿪵�Session
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e) {
            NHibernateSessionManager.Instance.CloseAllSession();
            /*
            OpenSessionInViewSection openSessionInViewSection = GetOpenSessionInViewSection();

            try {
                // Commit every session factory that's holding a transactional session
                foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories) {
                    if (sessionFactorySettings.IsTransactional) {
                        NHibernateSessionManager.Instance.CommitTransactionOn(sessionFactorySettings.FactoryConfigPath);
                    }
                }
            }
            finally {
                
                // No matter what happens, make sure all the sessions get closed
                //foreach (SessionFactoryElement sessionFactorySettings in openSessionInViewSection.SessionFactories) {
                //    NHibernateSessionManager.Instance.CloseSessionOn(sessionFactorySettings.FactoryConfigPath);
                //}
            }*/
        }

        private OpenSessionInViewSection GetOpenSessionInViewSection() {
            OpenSessionInViewSection openSessionInViewSection = ConfigurationManager
                .GetSection("nhibernateSettings") as OpenSessionInViewSection;

            Check.Ensure(openSessionInViewSection != null,
                "The nhibernateSettings section was not found with ConfigurationManager.");
            return openSessionInViewSection;
        }

        public void Dispose() { }
    }
}
