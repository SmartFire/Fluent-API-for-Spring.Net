using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context.Objects.Factory.Support
{
    public class ObjectDefinitionService : IObjectDefinitionService
    {
        private readonly IConfigurableListableObjectFactory _listableObjectFactory;
        private readonly IObjectDefinitionFactory _objectDefinitionFactory;

        public ObjectDefinitionService(IObjectDefinitionFactory objectDefinitionFactory, IConfigurableListableObjectFactory listableObjectFactory)
        {
            _objectDefinitionFactory = objectDefinitionFactory;
            _listableObjectFactory = listableObjectFactory;
        }

        #region IObjectDefinitionService Members

        public IObjectDefinitionFactory Factory
        {
            get { return _objectDefinitionFactory; }
        }

        public IConfigurableListableObjectFactory Registry
        {
            get { return _listableObjectFactory; }
        }

        #endregion
    }
}