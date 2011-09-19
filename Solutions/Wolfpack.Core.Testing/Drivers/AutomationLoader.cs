﻿using System;
using System.Collections.Generic;
using System.Linq;
using Wolfpack.Core.Interfaces;

namespace Wolfpack.Core.Testing.Drivers
{
    public class AutomationLoader<T> : ILoader<T>
    {
        private List<T> myItems;

        public AutomationLoader(params T[] items)
        {
            myItems = items.ToList();
        }

        public AutomationLoader(IEnumerable<T> items)
        {
            myItems = items.ToList();
        }

        public AutomationLoader<T> Add(T item)
        {
            myItems.Add(item);
            return this;
        }

        public bool Load(out T[] components)
        {
            components = myItems.ToArray();
            return (components.Length > 0);
        }

        public bool Load(out T[] components, Action<T> action)
        {
            var result = Load(out components);
            components.ToList().ForEach(action);
            return result;
        }
    }
}