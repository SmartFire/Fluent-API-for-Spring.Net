using System;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Configuration
{
    public interface IRegistrableObject
    {
        /// <summary>
        /// Gets the identifier name of the registered type. By default it will be the domain object type full name.
        /// </summary>
        /// <value>The identifier.</value>
        string Identifier { get; }

        /// <summary>
        /// Gets the type of the domain object registered.
        /// </summary>
        /// <value>The type of the domain object.</value>
        Type DomainObjectType { get; }

        /// <summary>
        /// Gets the object definition.
        /// </summary>
        /// <param name="objectDefinitionService">The object definition service.</param>
        /// <returns></returns>
        IObjectDefinition GetObjectDefinition(IObjectDefinitionService objectDefinitionService);
    }
}