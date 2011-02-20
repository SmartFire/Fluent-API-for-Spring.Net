using System;
using System.Collections.Generic;
using System.Configuration;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Configuration.Constraints
{
    internal class ConstrainableDuringLoadFactoryObject : AbstractFactoryObject
    {
        private IList<ConditionalObjectDefinition> conditionObjectDefinitions = new List<ConditionalObjectDefinition>();

        public IList<ConditionalObjectDefinition> ConditionObjectDefinitions
        {
            get { return conditionObjectDefinitions; }
            set { conditionObjectDefinitions = value; }
        }

        public override Type ObjectType
        {
            get { return GetObjectDefinitionRealType(); }
        }

        protected override object CreateInstance()
        {
            return GetObjectDefinition().Instance;
        }

        private Type GetObjectDefinitionRealType()
        {
            foreach (ConditionalObjectDefinition conditionalObjectDefinition in ConditionObjectDefinitions)
            {
                if (conditionalObjectDefinition.Condition())
                {
                    return Type.GetType(conditionalObjectDefinition.TypeName);
                }
            }
            throw new ConfigurationErrorsException(string.Format("could not locate runtime instance for the requested interface"));
        }

        private ConditionalObjectDefinition GetObjectDefinition()
        {
            foreach (ConditionalObjectDefinition conditionalObjectDefinition in ConditionObjectDefinitions)
            {
                if (conditionalObjectDefinition.Condition())
                {
                    return conditionalObjectDefinition;
                }
            }
            throw new ConfigurationErrorsException(string.Format("could not locate runtime instance for the requested interface"));
        }
    }
}