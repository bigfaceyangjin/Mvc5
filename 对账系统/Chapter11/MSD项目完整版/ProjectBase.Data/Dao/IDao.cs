using System;
using System.Collections.Generic;
using System.Text;
using ProjectBase.Utils.Entities;

namespace ProjectBase.Data
{
    /// <summary>
    /// �ṩ�����ݶԻ�������ɾ���ģ�����������ݽӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="IdT"></typeparam>
    public interface IDao<T, TID>
    {
        /// <summary>
        /// ����ID�����ݿ��ȡһ������ΪT��ʵ��
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns>����ID����Ӧ�Ķ���</returns>
        T GetById(TID id);

        /// <summary>
        /// ����ID�����ݿ��ȡһ������ΪT��ʵ��
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="shouldLock">�Ƿ�ʹ������ģʽ���ؽ��</param>
        /// <returns>����ID����Ӧ�Ķ���</returns>
        T GetById(TID id, bool shouldLock);

        /// <summary>
        /// �������еĶ���
        /// </summary>
        /// <returns>�������еĶ���</returns>
        IList<T> GetAll();

        /// <summary>
        /// ����exampleInstance�����Ҷ��󣬷�������ֵһ���Ķ����б�exampleInstance��ֵΪ0��NULL�����Խ�����Ϊ��������
        /// </summary>
        /// <param name="exampleInstance">�ο�����</param>
        /// <param name="propertiesToExclude">Ҫ�ų��Ĳ�ѯ����������</param>
        /// <returns></returns>
        IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// ����exampleInstance�����Ҷ��󣬷��ط�ҳ������ֵһ���Ķ����б�exampleInstance��ֵΪ0��NULL�����Խ�����Ϊ��������
        /// </summary>
        /// <param name="exampleInstance">�ο�����</param>
        /// <param name="pageIndex">��ʼҳ�룬��0��ʼ��¼</param>
        /// <param name="pageSize">ÿҳ��ʾ����</param>
        /// <param name="propertiesToExclude">Ҫ�ų��Ĳ�ѯ����������</param>
        /// <returns></returns>
        IPageOfList<T> GetByExample(T exampleInstance, int pageIndex, int pageSize, params string[] propertiesToExclude);

        /// <summary>
        /// ����exampleInstance�����Ҷ��󣬷�������ֵһ����Ψһ����exampleInstance��ֵΪ0��NULL�����Խ�����Ϊ��������
        /// </summary>
        /// <param name="exampleInstance">�ο�����</param>
        /// <param name="propertiesToExclude">Ҫ�ų��Ĳ�ѯ����������</param>
        /// <returns></returns>
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// ��ָ���Ķ��󱣴浽���ݿ�
        /// </summary>
        /// <param name="entity">Ҫ����Ķ���</param>
        /// <returns>���º�Ķ���</returns>
        T Save(T entity);

        /// <summary>
        /// ��ָ���Ķ��󱣴����µ����ݿ�
        /// </summary>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// �����ݿ���ɾ��ָ���Ķ���
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// �����Զ���Ĳ�ѯ������ѯ���
        /// </summary>
        /// <param name="filter">����</param>
        /// <returns></returns>
        IPageOfList<T> GetByFilter(ParameterFilter filter);

        /// <summary>
        /// �����ύ�޸ĵ����ݿ�
        /// </summary>
        void CommitChanges();

        DbTransaction BeginTransaction();
    }
}
