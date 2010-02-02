using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping.Rules;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapper
    {
        private List<AutoMapType> mappingTypes;
        private readonly IAutomappingDiscoveryRules rules;
        readonly IConventionFinder conventionFinder;
        private readonly IEnumerable<InlineOverride> inlineOverrides;
        private readonly IAutomappingStepSet steps;
        private readonly EntityAutomapper entityAutomapper;

        public AutoMapper(IAutomappingStepSet steps, IAutomappingDiscoveryRules rules, IConventionFinder conventionFinder, IEnumerable<InlineOverride> inlineOverrides)
            : this(steps, rules, conventionFinder, inlineOverrides, new EntityAutomapper(steps, conventionFinder))
        {}

        protected AutoMapper(IAutomappingStepSet steps, IAutomappingDiscoveryRules rules, IConventionFinder conventionFinder, IEnumerable<InlineOverride> inlineOverrides, EntityAutomapper entityAutomapper)
        {
            this.steps = steps;
            this.rules = rules;
            this.conventionFinder = conventionFinder;
            this.inlineOverrides = inlineOverrides;
            this.entityAutomapper = entityAutomapper;
            this.entityAutomapper.AutoMapper = this; // TODO: Remove this dependency
        }

        private void ApplyOverrides(Type classType, IList<string> mappedProperties, ClassMappingBase mapping)
        {
            var autoMapType = typeof(AutoMapping<>).MakeGenericType(classType);
            var autoMap = Activator.CreateInstance(autoMapType, mappedProperties);

            inlineOverrides
                .Where(x => x.CanOverride(classType))
                .Each(x => x.Apply(autoMap));

            ((IAutoClasslike)autoMap).AlterModel(mapping);
        }

        public ClassMappingBase MergeMap(Type classType, ClassMappingBase mapping, IList<string> mappedProperties)
        {
            // map class first, then subclasses - this way subclasses can inspect the class model
            // to see which properties have already been mapped
            ApplyOverrides(classType, mappedProperties, mapping);

            entityAutomapper.Map(mapping, classType, mappedProperties);

            if (mappingTypes != null)
                MapInheritanceTree(classType, mapping, mappedProperties);

            return mapping;
        }

        private void MapInheritanceTree(Type classType, ClassMappingBase mapping, IList<string> mappedProperties)
        {
            var discriminatorSet = false;
            var isDiscriminated = rules.FindDiscriminatedEntityRule(classType);

            foreach (var inheritedClass in mappingTypes.Where(q =>
                q.Type.BaseType == classType &&
                    !rules.FindConcreteBaseTypeRule(q.Type.BaseType)))
            {
                if (isDiscriminated && !discriminatorSet && mapping is ClassMapping)
                {
                    var discriminatorColumn = rules.DiscriminatorColumnRule(classType);
                    var discriminator = new DiscriminatorMapping
                    {
                        ContainingEntityType = classType,
                        Type = new TypeReference(typeof(string))
                    };
                    discriminator.AddDefaultColumn(new ColumnMapping { Name = discriminatorColumn });

                    ((ClassMapping)mapping).Discriminator = discriminator;
                    discriminatorSet = true;
                }

                ISubclassMapping subclassMapping;
                var subclassStrategy = rules.SubclassStrategyRule(classType);

                if (subclassStrategy == SubclassStrategy.JoinedSubclass)
                {
                    // TODO: This id name should be removed. Ideally it needs to be set by a
                    // default and be overridable by a convention (preferably the ForeignKey convention
                    // that already exists)
                    var subclass = new JoinedSubclassMapping
                    {
                        Type = inheritedClass.Type
                    };

                    subclass.Key = new KeyMapping();
                    subclass.Key.AddDefaultColumn(new ColumnMapping { Name = mapping.Type.Name + "_id" });

                    subclassMapping = subclass;
                }
                else
                    subclassMapping = new SubclassMapping();

				// track separate set of properties for each sub-tree within inheritance hierarchy
            	var subClassProperties = new List<string>(mappedProperties);
				MapSubclass(subClassProperties, subclassMapping, inheritedClass);

                mapping.AddSubclass(subclassMapping);

				MergeMap(inheritedClass.Type, (ClassMappingBase)subclassMapping, subClassProperties);
            }
        }

        private void MapSubclass(IList<string> mappedProperties, ISubclassMapping subclass, AutoMapType inheritedClass)
        {
            subclass.Name = inheritedClass.Type.AssemblyQualifiedName;
            ApplyOverrides(inheritedClass.Type, mappedProperties, (ClassMappingBase)subclass);
            entityAutomapper.Map((ClassMappingBase)subclass, inheritedClass.Type, mappedProperties);
            inheritedClass.IsMapped = true;
        }

        public ClassMapping Map(Type classType, List<AutoMapType> types)
        {
            var classMap = new ClassMapping { Type = classType };

            classMap.SetDefaultValue(x => x.Name, classType.AssemblyQualifiedName);
            classMap.SetDefaultValue(x => x.TableName, GetDefaultTableName(classType));

            mappingTypes = types;
            return (ClassMapping)MergeMap(classType, classMap, new List<string>());
        }

        private string GetDefaultTableName(Type type)
        {
            var tableName = type.Name;

            if (type.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = type.Name.Substring(0, type.Name.IndexOf('`'));

                foreach (var argument in type.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }

            return "`" + tableName + "`";
        }

        /// <summary>
        /// Flags a type as already mapped, stop it from being auto-mapped.
        /// </summary>
        public void FlagAsMapped(Type type)
        {
            mappingTypes
                .Where(x => x.Type == type)
                .Each(x => x.IsMapped = true);
        }
    }
}
