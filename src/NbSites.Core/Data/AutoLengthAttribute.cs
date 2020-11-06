using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NbSites.Core.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoLengthAttribute : Attribute
    {
        public AutoLengthCategory Category { get; set; }

        public static int StringPkMax = 100;
        public static int StringNormalMax = 200;
        public static int StringContentMax = 2000;

        public static int GetMax(AutoLengthCategory category)
        {
            if (category == AutoLengthCategory.StringPk)
            {
                return StringPkMax;
            }
            if (category == AutoLengthCategory.StringNormal)
            {
                return StringNormalMax;
            }
            if (category == AutoLengthCategory.StringContent)
            {
                return StringContentMax;
            }

            return StringNormalMax;
        }

        public int GetMax()
        {
            return GetMax(Category);
        }
    }

    public enum AutoLengthCategory
    {
        StringPk,
        StringNormal,
        StringContent
    }

    public static class AutoLengthAttributeExtensions
    {
        public static IDictionary<string, int> Maps { get; set; } = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public static ModelBuilder ApplyAutoLength(this ModelBuilder modelBuilder)
        {
            if (Maps.Count == 0)
            {
                Maps.Add("Id", AutoLengthAttribute.GetMax(AutoLengthCategory.StringPk));
                Maps.Add("Pk", AutoLengthAttribute.GetMax(AutoLengthCategory.StringPk));

                Maps.Add("Name", AutoLengthAttribute.GetMax(AutoLengthCategory.StringNormal));
                Maps.Add("Title", AutoLengthAttribute.GetMax(AutoLengthCategory.StringNormal));

                Maps.Add("Description", AutoLengthAttribute.GetMax(AutoLengthCategory.StringContent));
                Maps.Add("Json", AutoLengthAttribute.GetMax(AutoLengthCategory.StringContent));
                Maps.Add("Bag", AutoLengthAttribute.GetMax(AutoLengthCategory.StringContent));
                Maps.Add("Content", AutoLengthAttribute.GetMax(AutoLengthCategory.StringContent));
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var entityProperty in entityType.GetProperties())
                {
                    //Debug.WriteLine(entityType.ClrType + "." + entityProperty.Name + " "  + entityProperty);
                    if (!entityProperty.IsShadowProperty() && entityProperty.PropertyInfo != null)
                    {
                        //todo: more config by convention
                        if (entityProperty.ClrType == typeof(string))
                        {
                            SetStringLength(modelBuilder, entityType, entityProperty, entityProperty.PropertyInfo);
                        }
                    }
                }
            }

            return modelBuilder;
        }

        private static void SetStringLength(ModelBuilder modelBuilder, IMutableEntityType  entityType, IMutableProperty entityProperty, PropertyInfo propertyInfo)
        {
            var maxLength = entityProperty.GetMaxLength();
            if (maxLength == null)
            {
                modelBuilder.Entity(entityType.Name, b =>
                {
                    var propertyBuilder = b.Property(propertyInfo.Name);

                    var attributes = propertyInfo.GetCustomAttributes(typeof(AutoLengthAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var att = (AutoLengthAttribute)attributes[0];
                        propertyBuilder.HasMaxLength(att.GetMax());
                    }
                    else
                    {
                        var theKey = Maps.Keys.FirstOrDefault(x => propertyInfo.Name.EndsWith(x));
                        if (theKey != null)
                        {
                            propertyBuilder.HasMaxLength(Maps[theKey]);
                        }
                        else
                        {
                            propertyBuilder.HasMaxLength(AutoLengthAttribute.GetMax(AutoLengthCategory.StringNormal));
                        }
                    }
                });
            }
        }
    }
}
