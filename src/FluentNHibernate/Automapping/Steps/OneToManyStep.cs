using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class OneToManyStep : IAutomappingStep
    {
        private readonly IAutomappingDiscoveryRules rules;

        public OneToManyStep(IAutomappingDiscoveryRules rules)
        {
            this.rules = rules;
        }

        public bool IsMappable(Member property)
        {
            return rules.FindOneToManyRule(property);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (member.DeclaringType != classMap.Type)
                return;

            var mapping = GetCollectionMapping(member.PropertyType);

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);

            SetRelationship(member, classMap, mapping);
            SetKey(member, classMap, mapping);

            classMap.AddCollection(mapping);        
        }

        private ICollectionMapping GetCollectionMapping(Type type)
        {
            if (type.Namespace.StartsWith("Iesi.Collections") || type.Closes(typeof(HashSet<>)))
                return new SetMapping();

            return new BagMapping();
        }

        private void SetRelationship(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var relationship = new OneToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]),
                ContainingEntityType = classMap.Type
            };

            mapping.SetDefaultValue(x => x.Relationship, relationship);
        }

        private void SetKey(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "_id";

            if (classMap is ComponentMapping)
                columnName = rules.ComponentColumnPrefixRule(((ComponentMapping)classMap).Member) + columnName;

            var key = new KeyMapping();

            key.ContainingEntityType = classMap.Type;
            key.AddDefaultColumn(new ColumnMapping { Name = columnName});

            mapping.SetDefaultValue(x => x.Key, key);
        }
    }
}