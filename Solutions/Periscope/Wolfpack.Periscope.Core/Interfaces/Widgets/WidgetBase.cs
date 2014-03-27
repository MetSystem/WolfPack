
using System;
using System.Collections.Generic;
using System.IO;
using Wolfpack.Periscope.Core.Extensions;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Widgets
{
    public abstract class WidgetBase<T> : IWidget<T>
        where T: WidgetConfiguration, new()
    {
        public IWidgetBootstrapper Bootstrapper { get; set; }
        public List<Property> Includes { get; set; }
        public WidgetDefinition Definition { get; protected set; }
        public T Configuration { get; protected set; }

        public abstract void RenderMarkup(TextWriter writer);
        public abstract void RenderScript(TextWriter writer);

        protected WidgetBase()
        {
            Includes = new List<Property>();
            Definition = new WidgetDefinition
            {
                ImplementationType = GetType().BuildTypeName()
            };
        }
        public virtual IWidget<T> Configure(Action<T> configuratron, Func<T, IWidgetBootstrapper> builder = null)
        {
            var config = new T();
            configuratron(config);
            Configuration = config;

            if (builder != null)
            {
                var bootstrapper = builder(Configuration);
                if (bootstrapper != null)
                    Bootstrapper = bootstrapper;
            }

            return this;
        }

        public WidgetInstance CreateInstance()
        {
            var instance = new WidgetInstance
            {                
                Includes = Includes,
                Bootstrapper = Bootstrapper,
                Configuration = Configuration,
                Definition = Definition

            };

            using (var writer = new StringWriter())
            {
                RenderMarkup(writer);
                instance.Markup = writer.ToString();
            }
            using (var writer = new StringWriter())
            {
                RenderScript(writer);
                instance.Script = writer.ToString();
            }

            return instance;
        }

        protected void RegisterInclude(string name, string link)
        {
            Includes.Add(new Property{ Name = name, Value = link });
        }
    }
}