using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NbSites.Base.Data
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ToBaseTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, params string[] tableNamePath) where TEntity : class
        {
            var list = new List<string>();
            list.Add("Base");
            list.AddRange(tableNamePath);
            entityTypeBuilder.ToTableWithPath(list.ToArray());
            return entityTypeBuilder;
        }

        public static EntityTypeBuilder<TEntity> ToAppTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder,  params string[] tableNamePath) where TEntity : class
        {
            var list = new List<string>();
            list.Add("App");
            list.AddRange(tableNamePath);
            entityTypeBuilder.ToTableWithPath(list.ToArray());
            return entityTypeBuilder;
        }

        public static EntityTypeBuilder<TEntity> ToTableWithPath<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, params string[] tableNamePath) where TEntity : class
        {
            var @join = string.Join("_", tableNamePath);
            entityTypeBuilder.ToTable(@join);
            return entityTypeBuilder;
        }
    }
    
    //public class BaseConst
    //{
    //    public string Base { get; set; } = "Base";

    //    public static BaseConst Instance = InstanceHold.Default.Const<BaseConst>();
    //}

    //public class InstanceHold
    //{
    //    public T Const<T>(Func<T> createDefault = null) where T : new()
    //    {
    //        var theKey = typeof(T).FullName;
    //        if (theKey == null)
    //        {
    //            throw new InvalidOperationException("无效的键值");
    //        }

    //        if (!Items.ContainsKey(theKey))
    //        {
    //            createDefault ??= () => new T();
    //            Items[theKey] = createDefault();
    //        }

    //        return (T)Items[theKey];
    //    }

    //    public IDictionary<string, object> Items { get; set; } = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    //    public static InstanceHold Default = new InstanceHold();
    //}
}
