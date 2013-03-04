﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Wolfpack.Core.Interfaces.Entities
{
    [DebuggerDisplay("{Name}")]
    public class ConfigurationEntry
    {
        public class RequiredPropertyNames
        {
            public const string Name = "Name";
            public const string Scheduler = "Scheduler";
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string InterfaceType { get; set; }
        public string ConcreteType { get; set; }
        public string Data { get; set; }
        public List<string> Tags { get; set; }
        public Properties RequiredProperties { get; set; }

        public ConfigurationEntry()
        {
            RequiredProperties = new Properties();
            Tags = new List<string>();
        }
    }
}