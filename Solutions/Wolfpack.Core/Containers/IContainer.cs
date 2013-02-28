using System;
using System.Collections.Generic;

namespace Wolfpack.Core.Containers
{
    public interface IContainer
    {
        IContainer RegisterAsTransient(Type implType);
        IContainer RegisterAsTransient<T>(Type implType) where T : class;
        IContainer RegisterAsSingleton(Type implType);
        IContainer RegisterAsSingleton<T>(Type implType) where T : class;
        IContainer RegisterInstance<T>(T instance) where T : class;
        IContainer RegisterInstance<T>(T instance, string name) where T : class;
        IContainer RegisterInstance(Type implType, object instance, string name);
        IContainer RegisterAll<T>() where T : class;
        IContainer RegisterAllWithInterception<T, I>();
        object Resolve(string componentName);
        T Resolve<T>();        
        T[] ResolveAll<T>();
        T Find<T>(Func<IEnumerable<T>, T> filter);
        void ResolveAll<T>(Action<T> action);
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
    }
}