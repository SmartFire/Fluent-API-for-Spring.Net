#region License

/*
 * Copyright 2002-2006 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Collections.Specialized;
using System.IO;
using Common.Logging;
using Common.Logging.Log4Net;
using FluentSpring.Context;
using FluentSpring.Context.Support;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using Spring.Objects.Factory.Xml;

#endregion

namespace Spring.IocQuickStart.MovieFinder
{
    /// <summary>
    /// A simple application that demonstrates the IoC functionality of Spring.NET.
    /// </summary>
    /// <remarks>
    /// <p>
    /// See <a href="http://martinfowler.com/articles/injection.html"/> for the background
    /// article on which this example application is based.
    /// </p>
    /// </remarks>
    public sealed class Program
    {
        #region Logging Definition

        private static readonly ILog LOG = LogManager.GetLogger(typeof (Program));

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
                var ctx = CreateContextFluently();

                #region Five alternatives to create an applicationcontext.
                //var ctx = CreateDefaultContext();
                //var ctx = CreateContextFluently()
                //var ctx = CreateContextMixXmlAndProgrammatic();               
                //var ctx = CreateContextProgrammatically();                    
                //var ctx = CreateContextProgrammaticallyWithAutoWire();     
                #endregion

                var lister = (MovieLister) ctx.GetObject("MyMovieLister");
                Movie[] movies = lister.MoviesDirectedBy("Roberto Benigni");
                LOG.Debug("Searching for movie...");
                foreach (Movie movie in movies)
                {
                    LOG.Debug(
                        string.Format("Movie Title = '{0}', Director = '{1}'.",
                                      movie.Title, movie.Director));
                }
                LOG.Debug("MovieApp Done.");
            }
            catch (Exception e)
            {
                LOG.Error("Movie Finder is broken.", e);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("--- hit <return> to quit ---");
                Console.ReadLine();
            }
        }

        private static IApplicationContext CreateDefaultContext()
        {
            // if no context was registered before, 
            // then a call to GetContext() creates a new context from app.config:
            return ContextRegistry.GetContext();
        }

        #region Implementation of alternative ways to create application context

        private static IApplicationContext CreateContextFluently()
        {
            var ctx = new FluentGenericApplicationContext();

            FluentApplicationContext.Clear();

            FluentApplicationContext.Register<ColonDelimitedMovieFinder>("ColonDelimitedMovieFinder")
                .BindConstructorArgument<FileInfo>().To(new FileInfo("movies.txt"));

            FluentApplicationContext.Register<MovieLister>("MyMovieLister")
                .Bind(x => x.MovieFinder).To<IMovieFinder>("ColonDelimitedMovieFinder");

            FluentStaticConfiguration.ObjectDefinitionLoader.LoadObjectDefinitions(ctx.ObjectFactory);

            return ctx;
        }

        private static IApplicationContext CreateContextProgrammatically()
        {
            InitializeCommonLogging();
            var ctx = new GenericApplicationContext();

            IObjectDefinitionFactory objectDefinitionFactory = new DefaultObjectDefinitionFactory();


            //Create MovieLister and dependency on 
            ObjectDefinitionBuilder builder =
                ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, typeof (MovieLister));
            builder.AddPropertyReference("MovieFinder", "AnotherMovieFinder");

            ctx.RegisterObjectDefinition("MyMovieLister", builder.ObjectDefinition);

            builder = ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory,
                                                                   typeof (ColonDelimitedMovieFinder));
            builder.AddConstructorArg("movies.txt");
            ctx.RegisterObjectDefinition("AnotherMovieFinder", builder.ObjectDefinition);

            ctx.Refresh();

            return ctx;
        }

        private static IApplicationContext CreateContextProgrammaticallyWithAutoWire()
        {
            InitializeCommonLogging();
            var ctx = new GenericApplicationContext();

            IObjectDefinitionFactory objectDefinitionFactory = new DefaultObjectDefinitionFactory();


            //Create MovieLister and dependency on 
            ObjectDefinitionBuilder builder =
                ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, typeof (MovieLister));
            builder.AddPropertyReference("MovieFinder", "BogusNameOfDependency")
                .SetAutowireMode(AutoWiringMode.ByType);

            ctx.RegisterObjectDefinition("MyMovieLister", builder.ObjectDefinition);

            builder = ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory,
                                                                   typeof (ColonDelimitedMovieFinder));
            builder.AddConstructorArg("movies.txt")
                .SetAutowireMode(AutoWiringMode.ByType);

            ctx.RegisterObjectDefinition("AnotherMovieFinder", builder.ObjectDefinition);

            ctx.Refresh();

            return ctx;
        }

        private static IApplicationContext CreateContextMixXmlAndProgrammatic()
        {
            var ctx = new GenericApplicationContext();

            IObjectDefinitionReader objectDefinitionReader = new XmlObjectDefinitionReader(ctx);
            objectDefinitionReader.LoadObjectDefinitions(
                "assembly://Spring.IocQuickStart.MovieFinder/Spring.IocQuickStart.MovieFinder/AppContextContribution.xml");

            IObjectDefinitionFactory objectDefinitionFactory = new DefaultObjectDefinitionFactory();
            ObjectDefinitionBuilder builder =
                ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, typeof (ColonDelimitedMovieFinder));
            builder.AddConstructorArg("movies.txt");
            ctx.RegisterObjectDefinition("AnotherMovieFinder", builder.ObjectDefinition);


            ctx.Refresh();

            return ctx;
        }

        private static void InitializeCommonLogging()
        {
            var properties = new NameValueCollection();
            properties["configType"] = "INLINE";
            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(properties);
        }

        #endregion
    }
}