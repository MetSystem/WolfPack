using System.Collections.Generic;
using System.Text;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebUI
{
    public class DefaultMenuMarkupBuilder : IMenuMarkupBuilder
    {
        public string Build(IEnumerable<MenuItem> requests)
        {
            var sb = new StringBuilder();

            foreach (var menuItem in requests)
            {
                sb.Append(GetMarkup(menuItem));
            }

            return sb.ToString();
        }

        private string GetMarkup(MenuItem item)
        {
            switch (item.Type.ToLower())
            {
                case "dropdown":
                    return GetDropdownMarkup(item);

                case "item":
                    return GetItemMarkup(item);

                case "divider":
                    return GetDividerMarkup(item);
            }

            return string.Empty;
        }

        private string GetDropdownMarkup(MenuItem item)
        {
            /*
             <li class="dropdown">
				<a href="#" class="dropdown-toggle" data-toggle="dropdown">Tools <b class="caret"></b></a>
				<ul class="dropdown-menu">
					<li><a href="/ui/tools/diagnostics">Diagnostics</a></li>
					<li><a href="/ui/tools/sendnotification">Send Notification</a></li>
				</ul>
			</li>
             */
            var sb = new StringBuilder("<li class=\"dropdown\">");
            sb.AppendFormat("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">{0} <b class=\"caret\"></b></a>", item.Name);
            sb.Append("<ul class=\"dropdown-menu\">");

            foreach (var childItem in item.Children)
            {
                sb.Append(GetMarkup(childItem));
            }

            sb.Append("</ul>");
            sb.Append("</li>");
            return sb.ToString();
        }
        
        private string GetItemMarkup(MenuItem item)
        {
            /*
             <li><a href="/ui/configure">Configure</a></li>
             */
            return string.Format("<li><a hef=\"{0}\">{1}</a></li>", item.Url, item.Name);
        }
        
        private string GetDividerMarkup(MenuItem item)
        {
            /*
             <li><a href="/ui/configure">Configure</a></li>
             */
            return string.Format("<li class=\"divider\"></li>");
        }
    }
}