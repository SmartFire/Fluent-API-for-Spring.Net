using System;
using System.Collections;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Context.Objects.Factory
{
    public class CustomManagedList : ArrayList, IManagedCollection
    {


        public ICollection Resolve(string objectName, IObjectDefinition definition, string propertyName, ManagedCollectionElementResolver resolver)
        {
            throw new NotImplementedException();
        }
    }
}
