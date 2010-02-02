using System;

namespace FluentNHibernate.Automapping.Rules
{
    /// <summary>
    /// Container for automapping discovery rules.
    /// </summary>
    public interface IAutomappingDiscoveryRules
    {
        Predicate<Member> FindIdentityRule { get; }
        ManyToManyParentDelegate FindParentSideForManyToManyRule { get; }
        Predicate<Member> FindMappablePrivatePropertiesRule { get; }
        Predicate<Type> FindConcreteBaseTypeRule { get; }
        Predicate<Type> FindComponentRule { get; }
        ColumnNameDelegate ComponentColumnPrefixRule { get; }
        Predicate<Type> FindDiscriminatedEntityRule { get; }
        ClassColumnNameDelegate DiscriminatorColumnRule { get; }
        SubclassStrategyDelegate SubclassStrategyRule { get; }
        Predicate<Type> AbstractClassIsLayerSupertypeRule { get; }
        Predicate<Member> FindOneToManyRule { get; }
    }

    public delegate Type ManyToManyParentDelegate(Type leftSideType, Type rightSideType);
    public delegate string ColumnNameDelegate(Member member);
    public delegate string ClassColumnNameDelegate(Type type);
    public delegate SubclassStrategy SubclassStrategyDelegate(Type entityType);

    /// <summary>
    /// Base-class for setting your own discovery rules.
    /// </summary>
    public abstract class DiscoveryRules : IAutomappingDiscoveryRules
    {
        Predicate<Member> findIdentityRule;
        ManyToManyParentDelegate findParentSideForManyToManyRule;
        Predicate<Member> findMappablePrivatePropertiesRule;
        Predicate<Type> findConcreteBaseTypeRule;
        Predicate<Type> findComponentRule;
        ColumnNameDelegate componentColumnPrefixRule;
        Predicate<Type> findDiscriminatedEntityRule;
        ClassColumnNameDelegate discriminatorColumnRule;
        SubclassStrategyDelegate subclassStrategyRule;
        Predicate<Type> abstractClassIsLayerSupertypeRule;
        Predicate<Member> findOneToManyRule;

        public void FindIdentity(Predicate<Member> predicate)
        {
            findIdentityRule = predicate;
        }

        public void FindParentSideForManyToMany(ManyToManyParentDelegate match)
        {
            findParentSideForManyToManyRule = match;
        }

        public void FindMappablePrivateProperties(Predicate<Member> predicate)
        {
            findMappablePrivatePropertiesRule = predicate;
        }

        public void FindConcreteBaseType(Predicate<Type> predicate)
        {
            findConcreteBaseTypeRule = predicate;
        }

        public void FindComponent(Predicate<Type> predicate)
        {
            findComponentRule = predicate;
        }

        public void ComponentColumnPrefix(ColumnNameDelegate columnName)
        {
            componentColumnPrefixRule = columnName;
        }

        public void FindDiscriminatedEntity(Predicate<Type> predicate)
        {
            findDiscriminatedEntityRule = predicate;
        }

        public void DiscriminatorColumn(ClassColumnNameDelegate columnName)
        {
            discriminatorColumnRule = columnName;
        }

        public void GetSubclassStrategy(SubclassStrategyDelegate subclassStrategy)
        {
            subclassStrategyRule = subclassStrategy;
        }

        public void AbstractClassIsLayerSupertype(Predicate<Type> predicate)
        {
            abstractClassIsLayerSupertypeRule = predicate;
        }

        public void FindOneToMany(Predicate<Member> predicate)
        {
            findOneToManyRule = predicate;
        }

        Predicate<Member> IAutomappingDiscoveryRules.FindIdentityRule
        {
            get { return findIdentityRule; }
        }

        ManyToManyParentDelegate IAutomappingDiscoveryRules.FindParentSideForManyToManyRule
        {
            get { return findParentSideForManyToManyRule; }
        }

        Predicate<Member> IAutomappingDiscoveryRules.FindMappablePrivatePropertiesRule
        {
            get { return findMappablePrivatePropertiesRule; }
        }

        Predicate<Type> IAutomappingDiscoveryRules.FindConcreteBaseTypeRule
        {
            get { return findConcreteBaseTypeRule; }
        }

        Predicate<Type> IAutomappingDiscoveryRules.FindComponentRule
        {
            get { return findComponentRule; }
        }

        ColumnNameDelegate IAutomappingDiscoveryRules.ComponentColumnPrefixRule
        {
            get { return componentColumnPrefixRule; }
        }

        Predicate<Type> IAutomappingDiscoveryRules.FindDiscriminatedEntityRule
        {
            get { return findDiscriminatedEntityRule; }
        }

        ClassColumnNameDelegate IAutomappingDiscoveryRules.DiscriminatorColumnRule
        {
            get { return discriminatorColumnRule; }
        }

        SubclassStrategyDelegate IAutomappingDiscoveryRules.SubclassStrategyRule
        {
            get { return subclassStrategyRule; }
        }

        Predicate<Type> IAutomappingDiscoveryRules.AbstractClassIsLayerSupertypeRule
        {
            get { return abstractClassIsLayerSupertypeRule; }
        }
        
        Predicate<Member> IAutomappingDiscoveryRules.FindOneToManyRule
        {
            get { return findOneToManyRule; }
        }
    }
}