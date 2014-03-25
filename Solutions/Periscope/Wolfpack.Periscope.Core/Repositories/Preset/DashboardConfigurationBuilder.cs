using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Periscope.Core.Repositories.Preset
{
    public class DashboardConfigurationBuilder
    {
        private class ConfigurationInstruction
        {
            public string Name { get; set; }
            public Action<IDashboardInfrastructure, PanelBuilder> Configuratron { get; set; }
        }

        private readonly List<ConfigurationInstruction> _instructions;

        private DashboardConfigurationBuilder()
        {
            _instructions = new List<ConfigurationInstruction>();
        }

        public static DashboardConfigurationBuilder New()
        {
            return new DashboardConfigurationBuilder();
        }

        public DashboardConfigurationBuilder Add(string name, Action<IDashboardInfrastructure, PanelBuilder> configuratron)
        {
            var instruction = new ConfigurationInstruction
                                  {
                                      Name = name,
                                      Configuratron = configuratron
                                  };
            _instructions.Add(instruction);
            return this;
        }

        public DashboardConfiguration Build(IDashboardInfrastructure infrastructure)
        {
            var panels = _instructions.Select(
                i =>
                    {
                        var builder = new PanelBuilder(i.Name);
                        i.Configuratron(infrastructure, builder);
                        return builder.Build();
                    });

            return new DashboardConfiguration
                       {
                           Panels = panels
                       };
        }
    }
}