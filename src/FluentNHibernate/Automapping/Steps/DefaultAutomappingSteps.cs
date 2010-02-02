using System.Collections.Generic;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping.Steps
{
    public class DefaultAutomappingSteps : IAutomappingStepSet
    {
        readonly IAutomappingDiscoveryRules rules;

        public DefaultAutomappingSteps()
            : this(new DefaultDiscoveryRules())
        {}

        public DefaultAutomappingSteps(IAutomappingDiscoveryRules rules)
        {
            this.rules = rules;
        }

        public IEnumerable<IAutomappingStep> GetSteps(AutoMapper automapper, IConventionFinder conventionFinder)
        {
            return new IAutomappingStep[]
            {
                new IdentityStep(rules), 
                new VersionStep(), 
                new ComponentStep(rules, automapper),
                new PropertyStep(rules, conventionFinder),
                new ManyToManyStep(rules),
                new ManyToOneStep(),
                new OneToManyStep(rules),
            };
        }
    }
}