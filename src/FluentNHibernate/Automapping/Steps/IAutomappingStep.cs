using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public interface IAutomappingStep
    {
        bool IsMappable(Member member);
        void Map(ClassMappingBase classMap, Member member);
    }
}