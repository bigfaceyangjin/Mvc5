using ProjectBase.Data;
/****************************************************************************
 *Copyright (c) 2016 All Rights Reserved.
 *CLR版本： 4.0.30319.42000
 *机器名称：DESKTOP-V7CFIC3
 *公司名称：
 *命名空间：MSD.WL.Site.Filters
 *文件名：  CustomerFilter
 *版本号：  V1.0.0.0
 *唯一标识：638f64d9-3819-4a64-8bbc-8a9bb1513790
 *当前的用户域：DESKTOP-V7CFIC3
 *创建人：  zouqi
 *电子邮箱：zouyujie@126.com
 *创建时间：2016/10/7 18:24:18

 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2016/10/7 18:24:18
 *修改人： zouqi
 *版本号： V1.0.0.0
 *描述：
 *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.WL.Site.Filters
{
public class CustomerFilter : ParameterFilter
    {
        /// <summary>
        /// 客户代码
        /// </summary>
        public virtual string CusCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CusName { get; set; }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(CusCode))
            {
                hql += " and Cus_Code =:CusCode ";
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                hql += " and Cus_Name =:CusName ";
            }

            return hql;
        }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusCode))
            {
                result["CusCode"] = CusCode.Trim();
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                result["CusName"] = CusName.Trim();
            }
            return result;
        }
    }
}