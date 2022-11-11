using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;

namespace ProjectBase.Data
{
    public static class DaoExtensions
    {
        /// <summary>
        /// 为映射创建一个访问私有属性的表达式
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Expression<Func<TParent, TChild>> MakePrivateLambdaExpr<TParent, TChild>(this ClasslikeMapBase<TParent> parent,string propertyName)
        {
            var paramExpr = Expression.Parameter(typeof(TParent), typeof(TParent).Name);
            var propertyExpr = Expression.Property(paramExpr, propertyName);
            var castExpr = Expression.Convert(propertyExpr, typeof(TChild));
            var lambdaExpr = Expression.Lambda<Func<TParent, TChild>>(castExpr, paramExpr);
            return lambdaExpr;
        }
    }
}
