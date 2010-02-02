using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.Automapping.ManyToMany;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class ManyToManyAutomapperTester : BaseAutoMapFixture
    {
        [Test]
        public void CanMapManyToManyProperty()
        {
            var Member = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1).ToMember();
            var autoMap = new ClassMapping();

            var mapper = new ManyToManyStep(new DefaultDiscoveryRules());
            mapper.Map(autoMap, Member);

            autoMap.Collections.ShouldHaveCount(1);
        }

        [Test]
        public void GetsTableName()
        {
            Model<ManyToMany1>(model => model
                .Where(type => type == typeof(ManyToMany1)));

            Test<ManyToMany1>(mapping => mapping
                .Element("class/set")
                    .HasAttribute("table", "ManyToMany2ToManyToMany1"));
        }

    }
}
