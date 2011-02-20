using System;
using System.Collections.Generic;
using Spring.Objects.Factory.Config;

namespace FluentSpring.Tests.TestObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedPropertyResolver<T> : AbstractFactoryObject
    {
        private static string mapKey = "B";
        private IDictionary<string, T> propertyMap = new Dictionary<string, T>();

        private Type valueType;

        /// <summary>
        /// Gets or sets the T with the specified entry key.
        /// </summary>
        /// <value></value>
        public T this[string entryKey]
        {
            get { return PropertyMap[entryKey]; }
            set { PropertyMap[entryKey] = value; }
        }


        /// <summary>
        /// Gets or sets the property map.
        /// </summary>
        /// <value>The property map.</value>
        public IDictionary<string, T> PropertyMap
        {
            get { return propertyMap; }
            set { propertyMap = value; }
        }

        /// <summary>
        /// Gets or sets the map key.
        /// </summary>
        /// <value>The map key.</value>
        public string MapKey
        {
            get { return mapKey; }
            set { mapKey = value; }
        }


        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public override Type ObjectType
        {
            get
            {
                if (valueType == null)
                    return typeof (T);
                else
                    return valueType;
            }
        }


        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public string ValueType
        {
            get { return valueType.ToString(); }
            set
            {
                Type newType = Type.GetType(value);
                if (newType is T)
                    valueType = newType;
                else
                    throw new ArgumentException("ValueType property must be a subclass of " + typeof (T));
            }
        }

        protected override object CreateInstance()
        {
            if (propertyMap == null || mapKey == null)
                return null;
            if (!propertyMap.ContainsKey(mapKey))
                return null;
            return propertyMap[mapKey];
        }
    }
}