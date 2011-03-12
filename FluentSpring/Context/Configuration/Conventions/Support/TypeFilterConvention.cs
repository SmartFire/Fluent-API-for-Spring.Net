using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSpring.Context.Configuration.Conventions.Filters;


namespace FluentSpring.Context.Configuration.Conventions.Support
{
    public class TypeFilterConvention
    {
        private Func<Type, bool> _typeCondition;
        private Func<Assembly, bool> _assemblyCondition;

        public TypeFilterConvention()
        {
            _typeCondition = t => true;
            _assemblyCondition = a => true;
        }

        public ICanFilterAssembly For(Func<Type, bool> typeCondition)
        {
            _typeCondition = typeCondition;
            return null;
        }

        public void In(Func<Assembly, bool> assemblyCondition)
        {
            _assemblyCondition = assemblyCondition;
        }

        public IList<Type> GetAllTypes()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies() 
                        where _assemblyCondition(assembly) 
                            from type in assembly.GetTypes() 
                                where _typeCondition(type) select type)
                    .ToList();
        }
    }
}
