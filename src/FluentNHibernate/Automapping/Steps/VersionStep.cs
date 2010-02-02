using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Rules
{
    public class VersionStep : IAutomappingStep
    {
        private static readonly IList<string> ValidNames = new List<string> { "version", "timestamp" };
        private static readonly IList<Type> ValidTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan), typeof(byte[]) };

        public bool IsMappable(Member property)
        {
            return ValidNames.Contains(property.Name.ToLowerInvariant()) && ValidTypes.Contains(property.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (!(classMap is ClassMapping)) return;

            var version = new VersionMapping
            {
                Name = member.Name,
            };

            version.SetDefaultValue("Type", GetDefaultType(member));
            version.AddDefaultColumn(new ColumnMapping { Name = member.Name });

            if (IsSqlTimestamp(member))
            {
                version.Columns.Each(x =>
                {
                    x.SqlType = "timestamp";
                    x.NotNull = true;
                });
                version.UnsavedValue = null;
            }

            ((ClassMapping)classMap).Version = version;
        }

        private bool IsSqlTimestamp(Member property)
        {
            return property.PropertyType == typeof(byte[]);
        }

        private TypeReference GetDefaultType(Member property)
        {
            if (IsSqlTimestamp(property))
                return new TypeReference("BinaryBlob");

            return new TypeReference(property.PropertyType);
        }
    }
}