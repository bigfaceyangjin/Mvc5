using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectBase.Data
{
    /// <summary>
    /// ���ݱ����¼�
    /// </summary>
    public class SavedEventArgs : EventArgs
    {

        /// <summary>
        /// ��ʼ�� <see cref="SavedEventArgs"/> ��.
        /// </summary>
        /// <param name="action">��������.</param>
        public SavedEventArgs(SaveAction action)
        {
            Action = action;
        }

        private SaveAction _Action;
        /// <summary>
        /// ���ػ����ö�������.
        /// </summary>
        public SaveAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        public bool IsCancel
        {
            get;
            set;
        }
    }

    /// <summary>
    /// ��������ʱ�Ķ�������.
    /// </summary>
    public enum SaveAction
    {
        /// <summary>
        /// Ĭ��ֵ���޷�������.
        /// </summary>
        None,
        /// <summary>
        /// ��Ӷ���.
        /// </summary>
        Insert,
        /// <summary>
        /// ���¶���.
        /// </summary>
        Update,
        /// <summary>
        /// ɾ������.
        /// </summary>
        Delete
    }
}
