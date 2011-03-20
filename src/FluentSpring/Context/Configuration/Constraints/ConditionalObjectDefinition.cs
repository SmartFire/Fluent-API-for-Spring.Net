using System;

namespace FluentSpring.Context.Configuration.Constraints
{
    internal class ConditionalObjectDefinition
    {
        private readonly Func<bool> _condition;
        private readonly bool _isDefault;
        private readonly string _typeName;

        public ConditionalObjectDefinition(Func<bool> condition, string typeName, bool isDefault)
        {
            _condition = condition;
            _typeName = typeName;
            _isDefault = isDefault;
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public Func<bool> Condition
        {
            get { return _condition; }
        }

        public bool IsDefault
        {
            get { return _isDefault; }
        }

        public object Instance { get; set; }
    }
}