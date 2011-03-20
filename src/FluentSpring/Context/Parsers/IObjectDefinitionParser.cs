using System;
using FluentSpring.Context.Configuration;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Parsers
{
    public interface IObjectDefinitionParser
    {
        void AsNonSingleton();

        void SetPropertyReference(string propertyName, string identifier);
        void SetPropertyValue<T>(string propertyName, T propertyValue);
        void SetPropertyWithInlineDefinition(string propertyName, ICanContainConfiguration objectDefinitionConfiguration);

        void SetConstructorArgumentWithNameToReference(string construtorArgumentName, string identifier);
        void SetConstructorArgumentWithNameToValue<T>(string construtorArgumentName, T propertyValue);
        void SetConstructorArgumentWithNameToInlineDefinition(string construtorArgumentName, ICanContainConfiguration inlineConfiguration);

        void SetConstructorArgumentWithTypeToReference(Type construtorArgumentType, string identifier);
        void SetConstructorArgumentWithTypeToValue<T>(Type construtorArgumentType, T propertyValue);
        void SetConstructorArgumentWithTypeToInlineDefinition(Type construtorArgumentType, ICanContainConfiguration inlineConfiguration);

        void SetConstructorArgumentIndexToReference(int construtorArgumentIndex, string fullName);
        void SetConstructorArgumentIndexToValue<T>(int construtorArgumentIndex, T propertyValue);
        void SetConstructorArgumentWithIndexToInlineDefinition(int construtorArgumentIndex, ICanContainConfiguration inlineConfiguration);
        void SetFactoryMethod(string objectRegisteredName, string methodName);
        void AsAbstract();
        void SetDependencyLevel(DependencyCheckingMode dependencyMode);
        void AutoWire(AutoWiringMode wiringMode);
        void InitialiseObjectWithObjectMethod(string methodName);
        void DestroyObjectWith(string methodName);
        void IsLazyInitialised();
        void AddDependencyOn(string fullName);
        void AddParentDefinition(string parentDefinition);
    }
}