using System.Collections.Generic;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping.Steps
{
    public interface IAutomappingStepSet
    {
        // TODO: Remove the parameters as soon as possible
        IEnumerable<IAutomappingStep> GetSteps(AutoMapper automapper, IConventionFinder conventionFinder);
    }
}