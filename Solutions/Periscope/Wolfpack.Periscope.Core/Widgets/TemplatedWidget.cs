using System;
using System.Collections.Generic;
using System.IO;
using DotLiquid;
using Wolfpack.Periscope.Core.Extensions;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Widgets
{
    public class TemplatedWidget<T> : WidgetBase<T>
        where T : TemplatedWidgetConfiguration, new()
    {
        public override void RenderMarkup(TextWriter writer)
        {
            string templateContents;

            var filename = string.Format(@"widgets\templates\{0}.markup", Configuration.MarkupTemplate);
            using (var sr = new StreamReader(filename))
                templateContents = sr.ReadToEnd();

            var template = Template.Parse(templateContents);
            var templateResult = template.Render(Hash.FromDictionary(Configuration.TemplateData));
            writer.WriteLine(templateResult);
        }

        public override void RenderScript(TextWriter writer)
        {
            string templateContents;

            var filename = string.Format(@"widgets\templates\{0}.script", Configuration.ScriptTemplate);
            using (var sr = new StreamReader(filename))
                templateContents = sr.ReadToEnd();

            var template = Template.Parse(templateContents);
            var templateResult = template.Render(Hash.FromAnonymousObject(new { name = Configuration.Name }));
            writer.WriteLine(templateResult);
        }

        public override IWidget<T> Configure(Action<T> configuratron, Func<T, IWidgetBootstrapper> builder = null)
        {
            base.Configure(configuratron, builder);
            Includes = Configuration.Includes;
            return this;
        }
    }

    public class TemplatedWidgetConfiguration : WidgetConfiguration
    {
        private readonly IDictionary<string, object> _additionalTemplateData;
 
        public string MarkupTemplate { get; set; }
        public string ScriptTemplate { get; set; }
        public List<Property> Includes { get; set; }

        public IDictionary<string, object> TemplateData
        {
            get
            {
                var templateData = new Dictionary<string, object>(_additionalTemplateData);
                return templateData.AddFromAnonymous(new
                {
                    Name,
                    Width,
                    Height,
                    Title
                });
            }
        }

        public TemplatedWidgetConfiguration()
        {
            Includes = new List<Property>();

            _additionalTemplateData = new Dictionary<string, object>();
        }

        public TemplatedWidgetConfiguration AddTemplateData(object anonymous)
        {
            _additionalTemplateData.AddFromAnonymous(anonymous);
            return this;
        }
        public TemplatedWidgetConfiguration RegisterInclude(string name, string link)
        {
            Includes.Add(new Property { Name = name, Value = link });
            return this;
        }
    }
}