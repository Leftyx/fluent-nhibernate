using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.Conventions
{
    public class NullConventionFinder : IConventionFinder
    {
        public void AddAssembly(Assembly assembly) {}
        public void AddFromAssemblyOf<T>() {}
        public void Add<T>() where T : IConvention {}
        public void Add(Type type) {}
        public void Add(Type type, object instance) {}
        public void Add<T>(T instance) where T : IConvention {}
        
        public IEnumerable<T> Find<T>() where T : IConvention
        {
            return new T[0];
        }
    }
}