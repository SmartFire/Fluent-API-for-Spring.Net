using System;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Parsers
{
    public interface IDictionaryConfigurationParser
    {
        void Add(RuntimeObjectReference keyObjectReference, RuntimeObjectReference valueObjectReference);
        void Add<T>(T keyValue, RuntimeObjectReference runtimeObjectReference);
        void Add<V>(RuntimeObjectReference keyObjectReference, V actualValue);
        void Add<T, V>(T keyValue, V actualValue);
        void Add(RuntimeObjectReference keyObjectReference, Func<IObjectDefinitionService, object> runtimeObjectReference);
        void Add<T>(T keyValue, Func<IObjectDefinitionService, object> runtimeObjectReference);
        void Add(Func<IObjectDefinitionService, object> keyObjectReference, Func<IObjectDefinitionService, object> valueReference);
        void Add<T>(Func<IObjectDefinitionService, object> keyObjectReference, T actualValue);
        void Add<T>(Func<IObjectDefinitionService, object> keyObjectReference, RuntimeObjectReference actualValue);
    }
}