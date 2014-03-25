using System;
using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Infrastructure
{
    public interface IContainer
    {
        IContainer RegisterInstance<T>(T instance, Func<bool> predicate = null) where T : class;
        IContainer RegisterType<T>(Type type, Func<bool> predicate = null) where T : class;

        IContainer RegisterType<TInterface, TImplementation>(Func<bool> predicate = null)
            where TInterface : class
            where TImplementation : class, TInterface;

        IContainer RegisterMultiple<T>(IEnumerable<Type> implementations) where T : class;

        bool IsRegistered<T>() where T: class;

        T Resolve<T>() where T : class;
        IEnumerable<T> ResolveAll<T>() where T : class;
    }
}