using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Rules
{
    public class ComponentStep : IAutomappingStep
    {
        private readonly IAutomappingDiscoveryRules rules;
        private readonly AutoMapper mapper;

        public ComponentStep(IAutomappingDiscoveryRules rules, AutoMapper mapper)
        {
            this.rules = rules;
            this.mapper = mapper;
        }

        public bool IsMappable(Member property)
        {
            return rules.FindComponentRule(property.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            var mapping = new ComponentMapping
            {
                Name = member.Name,
                Member = member,
                ContainingEntityType = classMap.Type,
                Type = member.PropertyType
            };

            mapper.FlagAsMapped(member.PropertyType);
            mapper.MergeMap(member.PropertyType, mapping, new List<string>());

            classMap.AddComponent(mapping);
        }
    }
}