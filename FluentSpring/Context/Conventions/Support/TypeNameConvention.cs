using System;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Conventions.Support
{
    public class TypeNameConvention : IConvention
    {
        private readonly Func<Type, string> _typeToNameConvention;

        public TypeNameConvention() :
            this (t => t.FullName)
        {
        }

        public TypeNameConvention(Func<Type,string> typeToNameConvention)
        {
            _typeToNameConvention = typeToNameConvention;
        }

        public string GetIdentifierForType(Type objectType)
        {
            return _typeToNameConvention(objectType);
        }

        public bool IsApplicableToType(Type objectType)
        {
            throw new NotImplementedException();
        }

        public void ApplyConvention(string identifierName, IConfigurableObjectDefinition objectDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
