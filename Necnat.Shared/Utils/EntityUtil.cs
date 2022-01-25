using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Necnat.Shared.Utils
{
    public static class EntityUtil
    {
        public static TKey GetEntityKeyValue<TEntity, TKey>(TEntity e)
        {
            TKey ret = default(TKey);

            PropertyInfo key = typeof(TEntity).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            if (key != null)
                ret = (TKey)key.GetValue(e, null);

            return ret;
        }

        public static T SetEntityNotSealedTypePropertiesToNull<T>(T e)
        {
            foreach (var iPropertyInfo in e.GetType().GetProperties().Where(p => p.CanWrite))
                if (!iPropertyInfo.PropertyType.IsSealed)
                    iPropertyInfo.SetValue(e, null, null);

            return e;
        }
    }
}
