using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests.ClassMapSpecs
{
    public abstract class ClassMapAutomappingSpec
    {
        Establish context = () =>
            map = new ClassMap<Target>();

        protected static ClassMap<Target> map;
        protected static ClassMapping mapping;

        protected class Target
        {
            public string PropertyOne { get; set; }
            public string PropertyTwo { get; set; }
        }
    }

    public class when_class_map_is_told_to_automap_with_a_property_manually_mapped : ClassMapAutomappingSpec
    {
        Because of = () =>
        {
            map.Automap();
            map.Map(x => x.PropertyOne, "custom_column");
            mapping = map.As<IMappingProvider>().GetClassMapping();
        };

        It shouldnt_overwrite_the_manual_mapped_property = () =>
            mapping.Properties.First(x => x.Name == "PropertyOne").Columns.ShouldContain(x => x.Name == "custom_column");

        It should_automap_the_unmapped_property = () =>
            mapping.Properties.ShouldContain(x => x.Name == "PropertyTwo");

        It should_only_contain_property_mappings_for_the_two_properties_on_the_entity = () =>
            mapping.Properties.ShouldHaveCount(2);
    }

    public class when_class_map_is_told_to_automap : ClassMapAutomappingSpec
    {
        Because of = () =>
        {
            map.Automap();
            mapping = map.As<IMappingProvider>().GetClassMapping();
        };

        It should_map_properties = () =>
            mapping.Properties.ShouldHaveCount(2);

        It should_set_property_name_to_name_of_member = () =>
        {
            mapping.Properties.ShouldContain(x => x.Name == "PropertyOne");
            mapping.Properties.ShouldContain(x => x.Name == "PropertyTwo");
        };

        It should_set_property_column_name_to_name_of_member = () =>
        {
            mapping.Properties.First(x => x.Name == "PropertyOne").Columns.ShouldContain(x => x.Name == "PropertyOne");
            mapping.Properties.First(x => x.Name == "PropertyTwo").Columns.ShouldContain(x => x.Name == "PropertyTwo");
        };
    }
}