using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using NServiceBus;

namespace Wolfpack.Core.Containers
{
    public class WindsorContainer : IContainer
    {        
        protected Castle.Windsor.WindsorContainer myInstance =
            new Castle.Windsor.WindsorContainer(new ZeroAppConfigXmlInterpreter());

        public IContainer RegisterAsTransient(Type implType)
        {
            myInstance.Register(Component.For(implType)
                .ImplementedBy(implType)
                .LifeStyle.Transient);
            return this;
        }

        public IContainer RegisterAsTransient<T>(Type implType)
        {
            myInstance.Register(Component.For<T>()
                .ImplementedBy(implType)
                .LifeStyle.Transient);
            return this;
        }

        public IContainer RegisterAsSingleton(Type implType)
        {
            myInstance.Register(Component.For(implType)
                .ImplementedBy(implType)
                .LifeStyle.Singleton);
            return this;
        }

        public IContainer RegisterAsSingleton<T>(Type implType)
        {
            myInstance.Register(Component.For<T>()
                .ImplementedBy(implType)
                .LifeStyle.Singleton);
            return this;
        }

        public IContainer RegisterInstance<T>(T instance)
        {
            myInstance.Register(Component.For<T>().Instance(instance));
            return this;
        }

        public IContainer RegisterAll<T>()
        {
            Type[] components;

            if (TypeDiscovery.Discover<T>(out components))
                components.ForEach(c => RegisterAsTransient<T>(c));            
            return this;
        }

        public IContainer RegisterAllWithInterception<T, TI>()
        {
            Type[] components;

            if (!TypeDiscovery.Discover<T>(out components))
                return this;

            var interceptorTypes = (from iType in ResolveAll<TI>()
                                    select iType.GetType()).ToArray();

            components.ForEach(c =>
                               myInstance.Register(Component.For(typeof (T))
                                                       .LifeStyle.Transient
                                                       .ImplementedBy(c)
                                                       .Interceptors(interceptorTypes)));
            return this;
        }

        public object Resolve(string componentName)
        {
            return myInstance.Resolve(componentName, new Dictionary<string, string>());
        }

        public T Resolve<T>()
        {
            return myInstance.Resolve<T>();
        }

        public T[] ResolveAll<T>()
        {
            return myInstance.ResolveAll<T>();
        }

        public T Find<T>(Func<IEnumerable<T>, T> filter)
        {
            var components = ResolveAll<T>();
            return filter(components);
        }

        public void ResolveAll<T>(Action<T> action)
        {
            var components = ResolveAll<T>();
            components.ForEach(action);
        }

        public bool IsRegistered<T>()
        {
            return myInstance.Kernel.HasComponent(typeof (T));
        }

        public Configure Bus()
        {
            return Configure.With().CastleWindsorBuilder(myInstance);
        }

        public Configure Bus(params string[] assemblyNames)
        {
            var assemblies = from assemblyName in assemblyNames
                             select Assembly.Load(assemblyName);
            
            var listOfAssemblies = assemblies.ToList();

            // these are pre-requesites to allow NSB infrastructure
            // messaging (subscription etc) to operate
            listOfAssemblies.Add(Assembly.Load("NServiceBus"));
            listOfAssemblies.Add(Assembly.Load("NServiceBus.Core"));
            // finally register the assemblies with NSB and
            // register NSB with our Windsor container so that
            // any IBus dependencies are automatically resolved
            return Configure.With(listOfAssemblies).CastleWindsorBuilder(myInstance);
        }
    }
}