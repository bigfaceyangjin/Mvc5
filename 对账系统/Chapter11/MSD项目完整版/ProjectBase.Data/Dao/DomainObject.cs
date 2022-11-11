using ProjectBase.Utils;
using System.Threading;
using System.Text;
using System.Collections.Specialized;
using System.Web.Security;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace ProjectBase.Data
{
    /// <summary>
    /// �����ݳ־û�����������ģ�ͻ���
    /// </summary>
    /// <typeparam name="T">������������</typeparam>
    /// <typeparam name="TID">������������</typeparam>
    /// <typeparam name="TDao">���������Dao����</typeparam>
    public abstract class DomainObject<T, TID, TDao> : DomainObjectBase<TID>
        where T : DomainObject<T,TID,TDao>
        where TDao:IDao<T,TID>
    {
        protected static TDao Dao
        {
            get { return IocContainer.Instance.Resolve<TDao>(); }
        }

        #region Methods

        /// <summary>
        /// ���ݸ�����ID�ؽ�����
        /// </summary>
        /// <param name="id">�����Ψһ��ʶ</param>
        public static T Load(TID id)
        {
            return Dao.GetById(id);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public static IList<T> GetAll()
        {
            return Dao.GetAll();
        }

        /// <summary>
        /// �����ݿ�ɾ����ǰ����
        /// </summary>
        public virtual void Delete()
        {
            Dao.Delete((T)this);
        }

        /// <summary>
        /// �ѵ�ǰ������ӵ����ݿ�
        /// </summary>
        public virtual void Save()
        {
            if (!IsValid)
                return;

            SavedEventArgs result = OnSaving(this, SaveAction.Insert);
            if (result != null && result.IsCancel)
                return;
            Dao.Save((T)this);
            OnSaved(this, SaveAction.Insert);
        }

        public virtual void SaveWithTransaction()
        {
            using (var tran =Dao.BeginTransaction())
            {
                Save();
                tran.Commit();
            }
        }

        /// <summary>
        /// �ύ��ǰ������޸ĵ����ݿ�
        /// </summary>
        public virtual void Update()
        {
            if (!IsValid)
                return;

            SavedEventArgs result = OnSaving(this, SaveAction.Update);
            if (result != null && result.IsCancel)
                return;
            Dao.SaveOrUpdate((T)this);
            OnSaved(this, SaveAction.Update);
        }

        ///// <summary>
        ///// �ѵ�ǰ���󱣴浽���ݿ�
        ///// </summary>
        //virtual public void Save(string userName, string password)
        //{
        //    bool isValidated;
        //    if (userName == string.Empty)
        //        isValidated = false;
        //    else
        //        isValidated = Membership.ValidateUser(userName, password);

        //    Dao.Save((T)this);
        //}
        #endregion

        //����������޸�ʱ�Ĵ����¼�����ʱûʵ��
        #region Events

        /// <summary>
        /// �������������¼�
        /// </summary>
        public virtual event EventHandler<SavedEventArgs> Saved;
        /// <summary>
        /// ���� <see cref="Saved"/> �¼�.
        /// </summary>
        protected virtual void OnSaved(DomainObjectBase<TID> businessObject, SaveAction action)
        {
            if (Saved != null)
            {
                Saved(businessObject, new SavedEventArgs(action));
            }
        }

        /// <summary>
        /// �������ǰ�������¼�
        /// </summary>
        public virtual event EventHandler<SavedEventArgs> Saving;
        /// <summary>
        /// ���� <see cref="Saving"/> �¼�.
        /// </summary>
        protected virtual SavedEventArgs OnSaving(DomainObjectBase<TID> businessObject, SaveAction action)
        {
            if (Saving != null)
            {
                SavedEventArgs arge= new SavedEventArgs(action);
                Saving(businessObject, arge);
                return arge;
            }
            return null;
        }

        ///// <summary>
        ///// Occurs when this instance is marked dirty. 
        ///// It means the instance has been changed but not saved.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;
        ///// <summary>
        ///// Raises the PropertyChanged event safely.
        ///// </summary>
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        #endregion

        //#region IChangeTracking Members

        ///// <summary>
        ///// Resets the object�s state to unchanged by accepting the modifications.
        ///// </summary>
        //void IChangeTracking.AcceptChanges()
        //{
        //    Save();
        //}

        //virtual public bool IsChanged
        //{
        //    get { throw new NotImplementedException(); }
        //}
        //#endregion
        //*/
    }
}
