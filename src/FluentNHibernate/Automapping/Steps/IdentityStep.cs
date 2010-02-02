using System;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping.Steps
{
    public class IdentityStep : IAutomappingStep
    {
        private readonly IAutomappingDiscoveryRules rules;

        public IdentityStep(IAutomappingDiscoveryRules rules)
        {
            this.rules = rules;
        }

        public bool IsMappable(Member property)
        {
            return rules.FindIdentityRule(property);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (!(classMap is ClassMapping)) return;

            var idMapping = new IdMapping { ContainingEntityType = classMap.Type };
            idMapping.AddDefaultColumn(new ColumnMapping() { Name = member.Name });
            idMapping.Name = member.Name;
            idMapping.Type = new TypeReference(member.PropertyType);
            idMapping.Member = member;
            idMapping.SetDefaultValue("Generator", GetDefaultGenerator(member));
            ((ClassMapping)classMap).Id = idMapping;        
        }

        private GeneratorMapping GetDefaultGenerator(Member property)
        {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType);

            if (property.PropertyType == typeof(Guid))
                defaultGenerator.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                defaultGenerator.Identity();
            else
                defaultGenerator.Assigned();

            return generatorMapping;
        }
    }
}