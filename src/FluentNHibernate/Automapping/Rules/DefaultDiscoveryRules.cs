using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Rules
{
    public class DefaultDiscoveryRules : DiscoveryRules
    {
        public DefaultDiscoveryRules()
        {
            var rules = this as IAutomappingDiscoveryRules;

            FindIdentity(x => x.Name == "Id");
            FindParentSideForManyToMany((left, right) => left.FullName.CompareTo(right.FullName) < 0 ? left : right);
            FindConcreteBaseType(x => false);
            FindComponent(x => false);
            ComponentColumnPrefix(x => x.Name);
            DiscriminatorColumn(x => "discriminator");
            FindDiscriminatedEntity(t => rules.SubclassStrategyRule(t) == SubclassStrategy.Subclass);            GetSubclassStrategy(x => SubclassStrategy.JoinedSubclass);
            AbstractClassIsLayerSupertype(x => true);
            FindOneToMany(x => x.CanWrite && x.PropertyType.Namespace.In("System.Collections.Generic", "Iesi.Collections.Generic"));
        }
    }
}