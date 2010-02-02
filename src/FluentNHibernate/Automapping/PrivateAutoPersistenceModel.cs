using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            autoMapper = new PrivateAutoMapper(new DefaultAutomappingSteps(Expressions), Expressions, new DefaultConventionFinder(), inlineOverrides);
        }
    }
}