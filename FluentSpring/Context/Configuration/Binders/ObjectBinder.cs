using System;
using System.Configuration;
using System.Linq.Expressions;
using FluentSpring.Context.Configuration.Conventions;
using FluentSpring.Context.Parsers;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Configuration.Binders
{
    public class ObjectBinder<T> : ICanConfigureObject<T>
    {
        protected readonly ObjectDefinitionParser _configurationParser;

        public ObjectBinder(ObjectDefinitionParser configurationParser)
        {
            _configurationParser = configurationParser;
        }

        #region ICanConfigureObject<T> Members

        public ICanBindPropertyValue<T, X> Bind<X>(Expression<Func<T, X>> property)
        {
            return new PropertyValueBinder<T, X>(_configurationParser, GetPropertyName(property), this);    
        }

        public ICanBindPropertyValue<T, X> Bind<X>(string propertyName)
        {
            return new PropertyValueBinder<T, X>(_configurationParser, propertyName, this);
        }

        public ICanBindConstructorArgument<T, X> BindConstructorArgument<X>()
        {
            return new ConstructorArgumentBinder<T, X>(_configurationParser, typeof (X), this);
        }

        public ICanBindConstructorArgument<T, X> BindConstructorArgument<X>(string constructorArgumentName)
        {
            return new ConstructorArgumentBinder<T, X>(_configurationParser, constructorArgumentName, this);
        }

        public ICanBindConstructorArgument<T, X> BindConstructorArgumentAtIndex<X>(int index)
        {
            return new ConstructorArgumentBinder<T, X>(_configurationParser, index, this);
        }

        public ICanConfigureObject<T> InitialiseWith(Expression<Action<T>> methodInitialiser)
        {
            string methodName = ((MethodCallExpression) methodInitialiser.Body).Method.Name;
            _configurationParser.InitialiseObjectWithObjectMethod(methodName);
            return this;
        }

        public ICanContainConfiguration GetConfigurationParser()
        {
            return _configurationParser;
        }

        public ICanConfigureObject<T> AsNonSingleton()
        {
            _configurationParser.AsNonSingleton();
            return this;
        }

        public ICanConfigureObject<T> AsAbstract()
        {
            _configurationParser.AsAbstract();
            return this;
        }

        public ICanConfigureObject<T> AsPrototype()
        {
            _configurationParser.SetScope(ObjectScope.Prototype);
            return this;
        }

        public ICanConfigureObject<T> ForSession()
        {
            _configurationParser.SetScope(ObjectScope.Session);
            return this;
        }

        public ICanConfigureObject<T> ForRequest()
        {
            _configurationParser.SetScope(ObjectScope.Request);
            return this;
        }

        public ICanConfigureObject<T> ForApplication()
        {
            _configurationParser.SetScope(ObjectScope.Application);
            return this;
        }

        public ICanConfigureObject<T> IsLazyInitialised()
        {
            _configurationParser.IsLazyInitialised();
            return this;
        }

        public ICanConfigureObject<T> WithDependency(DependencyCheckingMode dependencyMode)
        {
            _configurationParser.SetDependencyLevel(dependencyMode);
            return this;
        }


        public ICanConfigureObject<T> IsCreatedWith<X>(Expression<Func<X, T>> factoryMethod)
        {
            if (!HasMethodGotParameters(factoryMethod))
            {
                string methodName = ((MethodCallExpression) factoryMethod.Body).Method.Name;
                _configurationParser.SetFactoryMethod(typeof (X).FullName, methodName);
            }
            else
            {
                throw new ConfigurationErrorsException(string.Format("You cannot use a factory method with parameters for this object {0}.", _configurationParser.DomainObjectType.FullName));
            }
            return this;
        }

        public ICanConfigureObject<T> AutoWire(AutoWiringMode wiringMode)
        {
            _configurationParser.AutoWire(wiringMode);
            return this;
        }

        public ICanConfigureObject<T> DestroyWith(Expression<Action<T>> methodDestroyer)
        {
            string methodName = ((MethodCallExpression) methodDestroyer.Body).Method.Name;
            _configurationParser.DestroyObjectWith(methodName);
            return this;
        }

        public ICanConfigureObject<T> DependsOn<X>()
        {
            _configurationParser.AddDependencyOn(typeof (X).FullName);
            return this;
        }

        public ICanConfigureObject<T> DependsOn<X>(string identifier)
        {
            _configurationParser.AddDependencyOn(identifier);
            return this;
        }

        public ICanConfigureObject<T> WithParentDefinition<X>()
        {
            _configurationParser.AddParentDefinition(typeof (X).FullName);
            return this;
        }

        public ICanConfigureObject<T> WithParentDefinition<X>(string identifier)
        {
            _configurationParser.AddParentDefinition(identifier);
            return this;
        }

        #endregion

        private static bool HasMethodGotParameters<X, V>(Expression<Func<X, V>> factoryMethod)
        {
            var methodExpression = (MethodCallExpression) factoryMethod.Body;
            return methodExpression.Arguments.Count > 0;
        }

        private static string GetPropertyName<X>(Expression<Func<T, X>> property)
        {
            if (property.Body is MemberExpression)
            {
                var propertyExpression = (MemberExpression)property.Body;
                return propertyExpression.Member.Name;
            }
            else
            {
                throw new Exception(string.Format("Unrecognisable method/property, use Bind<Type>(\"propertyname\") instead"));
            }
        }

        public ICanConfigureObject<T> Apply(IConvention convention) 
        {
            _configurationParser.ApplyConvention(convention);
            return this;
        }

    }
}