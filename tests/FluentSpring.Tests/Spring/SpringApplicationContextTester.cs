using System;
using System.Collections;
using System.Reflection;
using Spring.Context;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using Spring.Testing.NUnit;


namespace FluentSpring.Tests.Spring
{
    public class SpringApplicationContextTester : AbstractSpringContextTests
    {
        private readonly string[] _configLocations;

        /// <summary>
        /// Application context this test will run against.
        /// </summary>
        protected IConfigurableApplicationContext applicationContext;

        private AutoWiringMode autowireMode = AutoWiringMode.ByType;
        private bool dependencyCheck = true;

        private int loadCount;

        /// <summary>
        /// Holds names of the fields that should be used for field injection.
        /// </summary>
        protected string[] managedVariableNames;

        /// <summary>
        /// Default constructor for AbstractDependencyInjectionSpringContextTests.
        /// </summary>
        public SpringApplicationContextTester(string[] configLocations)
        {
            _configLocations = configLocations;

            PopulateProtectedVariables = false;
        }

        /// <summary>
        /// Gets or sets a flag specifying whether to populate protected 
        /// variables of this test case.
        /// </summary>
        /// <value>
        /// A flag specifying whether to populate protected variables of this test case. 
        /// Default is <b>false</b>.
        /// </value>
        public bool PopulateProtectedVariables { get; set; }

        /// <summary>
        /// Gets or sets the autowire mode for test properties set by Dependency Injection.
        /// </summary>
        /// <value>
        /// The autowire mode for test properties set by Dependency Injection.
        /// The default is <see cref="AutoWiringMode.ByType"/>.
        /// </value>
        public AutoWiringMode AutowireMode
        {
            get { return autowireMode; }
            set { autowireMode = value; }
        }

        /// <summary>
        /// Gets or sets a flag specifying whether or not dependency checking 
        /// should be performed for test properties set by Dependency Injection.
        /// </summary>
        /// <value>
        /// <p>A flag specifying whether or not dependency checking 
        /// should be performed for test properties set by Dependency Injection.</p>
        /// <p>The default is <b>true</b>, meaning that tests cannot be run
        /// unless all properties are populated.</p>
        /// </value>
        public bool DependencyCheck
        {
            get { return dependencyCheck; }
            set { dependencyCheck = value; }
        }

        /// <summary>
        /// Gets the current number of context load attempts.
        /// </summary>
        public int LoadCount
        {
            get { return loadCount; }
        }

        /// <summary>
        /// Gets a key for this context. Usually based on config locations, but
        /// a subclass overriding buildContext() might want to return its class.
        /// </summary>
        protected virtual object ContextKey
        {
            get { return ConfigLocations; }
        }

        protected string[] ConfigLocations
        {
            get { return _configLocations; }
        }

        /// <summary>
        /// Called to say that the "applicationContext" instance variable is dirty and
        /// should be reloaded. We need to do this if a test has modified the context
        /// (for example, by replacing an object definition).
        /// </summary>
        public void SetDirty()
        {
            SetDirty(ConfigLocations);
        }

        public IApplicationContext InitialiseContext()
        {
            applicationContext = GetContext(ContextKey);
            InjectDependencies();
            return applicationContext;
        }

        /// <summary>
        /// Inject dependencies into 'this' instance (that is, this test instance).
        /// </summary>
        /// <remarks>
        /// <p>The default implementation populates protected variables if the
        /// <see cref="PopulateProtectedVariables"/> property is set, else
        /// uses autowiring if autowiring is switched on (which it is by default).</p>
        /// <p>You can certainly override this method if you want to totally control
        /// how dependencies are injected into 'this' instance.</p>
        /// </remarks>
        protected virtual void InjectDependencies()
        {
            if (PopulateProtectedVariables)
            {
                if (managedVariableNames == null)
                {
                    InitManagedVariableNames();
                }
                InjectProtectedVariables();
            }
            else if (AutowireMode != AutoWiringMode.No)
            {
                IConfigurableListableObjectFactory factory = applicationContext.ObjectFactory;
                ((AbstractObjectFactory) factory).IgnoreDependencyType(typeof (AutoWiringMode));
                factory.AutowireObjectProperties(this, AutowireMode, DependencyCheck);
            }
        }

        /// <summary>
        /// Loads application context from the specified resource locations.
        /// </summary>
        /// <param name="locations">Resources to load object definitions from.</param>
        protected override IConfigurableApplicationContext LoadContextLocations(string[] locations)
        {
            ++loadCount;
            return base.LoadContextLocations(locations);
        }

        /// <summary>
        /// Retrieves the names of the fields that should be used for field injection.
        /// </summary>
        protected virtual void InitManagedVariableNames()
        {
            var managedVarNames = new ArrayList();
            Type type = GetType();

            do
            {
                FieldInfo[] fields =
                    type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Found " + fields.Length + " fields on " + type);
                }

                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = fields[i];
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Candidate field: " + field);
                    }
                    if (IsProtectedInstanceField(field))
                    {
                        object oldValue = field.GetValue(this);
                        if (oldValue == null)
                        {
                            managedVarNames.Add(field.Name);
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Added managed variable '" + field.Name + "'");
                            }
                        }
                        else
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Rejected managed variable '" + field.Name + "'");
                            }
                        }
                    }
                }
                type = type.BaseType;
            } while (type != typeof (AbstractDependencyInjectionSpringContextTests));

            managedVariableNames = (string[]) managedVarNames.ToArray(typeof (string));
        }

        private static bool IsProtectedInstanceField(FieldInfo field)
        {
            return field.IsFamily;
        }

        /// <summary>
        /// Injects protected fields using Field Injection.
        /// </summary>
        protected virtual void InjectProtectedVariables()
        {
            for (int i = 0; i < managedVariableNames.Length; i++)
            {
                string fieldName = managedVariableNames[i];
                Object obj = null;
                try
                {
                    FieldInfo field = GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        BeforeProtectedVariableInjection(field);
                        obj = applicationContext.GetObject(fieldName, field.FieldType);
                        field.SetValue(this, obj);
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Populated field: " + field);
                        }
                    }
                    else
                    {
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn("No field with name '" + fieldName + "'");
                        }
                    }
                }
                catch (NoSuchObjectDefinitionException)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("No object definition with name '" + fieldName + "'");
                    }
                }
            }
        }

        /// <summary>
        /// Called right before a field is being injected
        /// </summary>
        protected virtual void BeforeProtectedVariableInjection(FieldInfo fieldInfo)
        {
        }
    }
}