using System;
using System.Linq.Expressions;

namespace Devcat
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ValueCastTo
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // author: netics@nexon.co.kr
    public class ValueCastTo
    {
        protected static class Cache<TFrom, TTo>
        {
            public static readonly Func<TFrom, TTo> Caster = Get();

            static Func<TFrom, TTo> Get()
            {
                var p = Expression.Parameter(typeof(TFrom), "from");
                var c = Expression.ConvertChecked(p, typeof(TTo));
                return Expression.Lambda<Func<TFrom, TTo>>(c, p).Compile();
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ValueCastTo<T>
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ValueCastTo<TTo> : ValueCastTo
    {
        //--------------------------------------------------------------------------------------------------------------------------------
        public static TTo From<TFrom>(TFrom from)
        {
            return Cache<TFrom, TTo>.Caster(from);
        }
    }
}