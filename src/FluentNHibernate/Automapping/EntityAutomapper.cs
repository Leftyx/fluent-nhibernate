using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class EntityAutomapper
    {
        private readonly IAutomappingStepSet steps;
        private readonly IConventionFinder conventionFinder;

        public AutoMapper AutoMapper { get; set; }

        public EntityAutomapper(IAutomappingStepSet steps, IConventionFinder conventionFinder)
        {
            this.steps = steps;
            this.conventionFinder = conventionFinder;
        }

        public virtual void Map(ClassMappingBase mapping, Type entityType, IList<string> mappedProperties)
        {
            foreach (var property in entityType.GetProperties())
            {
                TryToMapProperty(mapping, property.ToMember(), mappedProperties);
            }
        }

        protected void TryToMapProperty(ClassMappingBase mapping, Member property, IList<string> mappedProperties)
        {
            if (property.HasIndexParameters) return;

            foreach (var rule in steps.GetSteps(AutoMapper, conventionFinder))
            {
                if (!rule.IsMappable(property)) continue;
                if (mappedProperties.Any(name => name == property.Name)) continue;

                rule.Map(mapping, property);
                mappedProperties.Add(property.Name);

                break;
            }
        }
    }
}