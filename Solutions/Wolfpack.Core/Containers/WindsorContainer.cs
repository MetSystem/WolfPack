using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Wolfpack.Core.Containers
{
    public class WindsorContainer : IContainer
    {
        protected Castle.Windsor.WindsorContainer _instance;

        public WindsorContainer()
        {
            _instance = new Castle.Windsor.WindsorContainer(new ZeroAppConfigXmlInterpreter());
            _instance.Kernel.Resolver.AddSubResolver(new CollectionResolver(_instance.Kernel));
        }

        public IContainer RegisterAsTransient(Type implType)
        {
            _instance.Register(Component.For(implType)
                .ImplementedBy(implType)
                .LifeStyle.Transient);
            return this;
        }

        public IContainer RegisterAsTransient<T>(Type implType)
            where T: class
        {
            _instance.Register(Component.For<T>()
                .ImplementedBy(implType)
                .LifeStyle.Transient);
            return this;
        }

        public IContainer RegisterAsSingleton(Type implType)
        {
            _instance.Register(Component.For(implType)
                .ImplementedBy(implType)
                .LifeStyle.Singleton);
            return this;
        }

        public IContainer RegisterAsSingleton<T>(Type implType)
            where T: class
        {
            _instance.Register(Component.For<T>()
                .ImplementedBy(implType)
                .LifeStyle.Singleton);
            return this;
        }

        public IContainer RegisterInstance<T>(T instance, bool overwrite = false)
            where T : class
        {
            _instance.Register(overwrite
                                   ? Component.For<T>().Instance(instance).OverWrite()
                                   : Component.For<T>().Instance(instance));
            return this;
        }

        public IContainer RegisterInstance<T>(T instance, string name)
            where T : class
        {
            _instance.Register(Component.For<T>().Instance(instance).Named(name));
            return this;
        }

        public IContainer RegisterInstance(Type implType, object instance, string name)
        {
            _instance.Register(Component.For(implType).Instance(instance).Named(name));
            return this;
        }

        public IContainer RegisterAll<T>()
            where T : class
        {
            Type[] components;

            if (TypeDiscovery.Discover<T>(out components))
                components.ToList().ForEach(c => RegisterAsTransient<T>(c));            
            return this;
        }

        public IContainer RegisterAllWithInterception<T, TI>()
        {
            Type[] components;

            if (!TypeDiscovery.Discover<T>(out components))
                return this;

            var interceptorTypes = (from iType in ResolveAll<TI>()
                                    select iType.GetType()).ToArray();

            components.ToList().ForEach(c =>
                               _instance.Register(Component.For(typeof (T))
                                                       .LifeStyle.Transient
                                                       .ImplementedBy(c)
                                                       .Interceptors(interceptorTypes)));
            return this;
        }

        public object Resolve(string componentName)
        {
            return _instance.Resolve<object>(componentName, new Dictionary<string, string>());
        }

        public T Resolve<T>()
        {
            return _instance.Resolve<T>();
        }

        public T[] ResolveAll<T>()
        {
            return _instance.ResolveAll<T>();
        }

        public T Find<T>(Func<IEnumerable<T>, T> filter)
        {
            var components = ResolveAll<T>();
            return filter(components);
        }

        public void ResolveAll<T>(Action<T> action)
        {
            var components = ResolveAll<T>();
            components.ToList().ForEach(action);
        }

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof (T));
        }

        public bool IsRegistered(Type type)
        {
            return _instance.Kernel.HasComponent(type);
        }
    }
}