using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Conventions;
using FluentSpring.Context.Extensions;
using FluentSpring.Context.Objects.Factory;
using Spring.Objects;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Parsers
{
    public class LambdaObjectDefinitionExpressionParser<T> : ICanContainConfiguration, IRegistrableObject
    {
        private readonly string _identifier;
        private bool _isSingleton = true;
        private Expression<Func<IObjectRegistry, T>> _objectCreation;
        private string _parentDefinition = string.Empty;

        public LambdaObjectDefinitionExpressionParser(string identifier)
        {
            _identifier = identifier;
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

        public Type DomainObjectType
        {
            get
            {
                return typeof(T);
            }
        }

        public IObjectDefinition GetObjectDefinition(IObjectDefinitionService objectDefinitionService)
        {
            // parse the expression tree to create spring object definition.
            IConfigurableObjectDefinition objectDefinition = CreateObjectDefinition(objectDefinitionService);

            switch (_objectCreation.Body.NodeType)
            {
                case ExpressionType.MemberInit:
                    MemberInit((MemberInitExpression) _objectCreation.Body, objectDefinition);
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.New:
                    New((NewExpression) _objectCreation.Body, objectDefinition);
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.ListInit:
                    break;
                // straight compilation of expression is required.
                default:
                    break;
            }

            return objectDefinition;
        }

        private void New(NewExpression body, IConfigurableObjectDefinition objectDefinition)
        {
            int indexer = 0;
            foreach (Expression expression in body.Arguments)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Call:
                        MethodCall((MethodCallExpression)expression, objectDefinition, indexer);
                        break;
                    case ExpressionType.Constant:
                        objectDefinition.ConstructorArgumentValues.AddIndexedArgumentValue(indexer, ((ConstantExpression)expression).Value);
                        break;

                    default:
                        break;
                }
                indexer++;
            }
        }

        private void MemberInit(MemberInitExpression body, IConfigurableObjectDefinition objectDefinition)
        {
            int indexer = 0;
            foreach (Expression expression in body.NewExpression.Arguments)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Call:
                        MethodCall((MethodCallExpression) expression, objectDefinition, indexer);
                        break;
                    case ExpressionType.Constant:
                        objectDefinition.ConstructorArgumentValues.AddIndexedArgumentValue(indexer, ((ConstantExpression) expression).Value);
                        break;
                        
                    default:
                        break;
                }
                indexer++;
            }

            foreach (MemberBinding memberBinding in body.Bindings)
            {
                switch (memberBinding.BindingType)
                {
                    case MemberBindingType.Assignment:
                        MemberAssignment(memberBinding.Member.Name, (MemberAssignment) memberBinding, objectDefinition);
                        break;
                    case MemberBindingType.ListBinding:
                        break;
                    case MemberBindingType.MemberBinding:
                        break;
                }
            }
        }

        private void MemberAssignment(string propertyName, MemberAssignment memberBinding, IConfigurableObjectDefinition objectDefinition)
        {
            switch(memberBinding.Expression.NodeType)
            {
                case ExpressionType.Call:
                    MethodCall(propertyName, (MethodCallExpression) memberBinding.Expression, objectDefinition);
                    break;
                case ExpressionType.Constant:
                    objectDefinition.PropertyValues.Add(propertyName, ((ConstantExpression)memberBinding.Expression).Value);
                    break;
                case ExpressionType.ListInit:
                    ListInit(propertyName, ((ListInitExpression) memberBinding.Expression), objectDefinition);
                    break;
                case ExpressionType.NewArrayInit:
                    ArrayInit(propertyName, ((NewArrayExpression) memberBinding.Expression), objectDefinition);
                    break;
            }
        }

        private void ArrayInit(string propertyName, NewArrayExpression newArrayExpression, IConfigurableObjectDefinition objectDefinition)
        {
            throw new NotImplementedException();
        }

        private void ListInit(string propertyName, ListInitExpression listInitExpression, IConfigurableObjectDefinition objectDefinition)
        {
            foreach (Type @interface in listInitExpression.Type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(IList<>))
                    {
                        ManagedList list = new ManagedList();

                        list.ElementTypeName = listInitExpression.Type.AssemblyQualifiedName;
                    }
                }
            }

        }


        private void MethodCall(MethodCallExpression expression, IConfigurableObjectDefinition objectDefinition, int indexer)
        {
            if (expression.Method.DeclaringType.Equals(typeof(IObjectRegistry)))
            {
                if (expression.Arguments.Count==1)
                {
                    objectDefinition.ConstructorArgumentValues.AddIndexedArgumentValue(indexer, new RuntimeObjectReference((string)((ConstantExpression)expression.Arguments[0]).Value));
                }
                else
                {
                    objectDefinition.ConstructorArgumentValues.AddIndexedArgumentValue(indexer, new RuntimeObjectReference(expression.Method.ReturnType.FullName));    
                }
                
            }
            else
            {
                
            }
        }

        private void MethodCall(string propertyName, MethodCallExpression expression, IConfigurableObjectDefinition objectDefinition)
        {
            if (expression.Method.DeclaringType.Equals(typeof(IObjectRegistry)))
            {
                if (expression.Arguments.Count == 1)
                {
                    objectDefinition.PropertyValues.Add(new PropertyValue(propertyName, new RuntimeObjectReference((string)((ConstantExpression)expression.Arguments[0]).Value)));
                }
                else
                {
                    objectDefinition.PropertyValues.Add(new PropertyValue(propertyName, new RuntimeObjectReference(expression.Method.ReturnType.FullName)));
                }
            }
            else
            {
                
            }
        }

        public void AddConvention(IConvention convention)
        {
            throw new NotImplementedException();
        }

        public void AsNonSingleton()
        {
            _isSingleton = false;
        }

        public void AddConstructionExpression(Expression<Func<IObjectRegistry, T>> objectCreation)
        {
            _objectCreation = objectCreation;
        }

        private IConfigurableObjectDefinition CreateObjectDefinition(IObjectDefinitionService objectDefinitionService)
        {
            IConfigurableObjectDefinition objectDefinition;
            if (String.IsNullOrEmpty(_parentDefinition))
            {
                objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(DomainObjectType.AssemblyQualifiedName, null, AppDomain.CurrentDomain);
            }
            else
            {
                objectDefinition = objectDefinitionService.Factory.CreateObjectDefinition(DomainObjectType.AssemblyQualifiedName, _parentDefinition, AppDomain.CurrentDomain);
            }
            return objectDefinition;
        }
    }
}
