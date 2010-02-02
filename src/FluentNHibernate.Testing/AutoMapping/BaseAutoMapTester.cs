using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Utils;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    public abstract class BaseAutoMapTester<T>
        where T : IAutomappingStep, new()
    {
        private T mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new T();
        }

        protected void ShouldMap(Expression<Func<PropertyTarget, object>> property)
        {
            mapper.IsMappable(ReflectionHelper.GetProperty(property).ToMember()).ShouldBeTrue();
        }

        protected void ShouldntMap(Expression<Func<PropertyTarget, object>> property)
        {
            mapper.IsMappable(ReflectionHelper.GetProperty(property).ToMember()).ShouldBeFalse();
        }

        protected class PropertyTarget
        {
            public ISet<PropertyTarget> Set { get; set; }
            public IList<PropertyTarget> List { get; set; }
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public PropertyTarget Entity { get; set; }
        }
    }
}