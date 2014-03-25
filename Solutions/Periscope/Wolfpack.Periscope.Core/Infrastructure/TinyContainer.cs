using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Infrastructure
{
    public class TinyContainer : IContainer
    {
        private readonly TinyIoCContainer _container = new TinyIoCContainer();

        public IContainer RegisterInstance<T>(T instance, Func<bool> predicate = null)
            where T : class
        {
            _container.Register(instance);
            return this;
        }

        public IContainer RegisterType<T>(Type type, Func<bool> predicate = null)
            where T : class
        {
            throw new NotImplementedException();
            //_container.Register<>()
        }

        public IContainer RegisterType<TInterface,TImplementation>(Func<bool> predicate = null) 
            where TInterface : class
            where TImplementation : class, TInterface
        {
            if (predicate != null)
            {
                if (predicate())
                    _container.Register<TInterface, TImplementation>();
                return this;
            }
            
            _container.Register<TInterface, TImplementation>();
            return this;
        }

        public IContainer RegisterMultiple<T>(IEnumerable<Type> implementations) where T : class
        {
            _container.RegisterMultiple(typeof(T), implementations );
            return this;
        }

        public bool IsRegistered<T>() where T : class
        {
            return _container.CanResolve<T>();
        }

        public T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return _container.ResolveAll<T>();
        }
    }
}