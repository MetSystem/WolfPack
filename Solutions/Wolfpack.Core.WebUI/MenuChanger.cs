using System.Collections.Generic;
using System.Linq;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebUI
{
    public class MenuChanger
    {
        private readonly IEnumerable<INeedMenuSpace> _menuChangers;
        private readonly IMenuMarkupBuilder _markupBuilder;

        public MenuChanger(IEnumerable<INeedMenuSpace> menuChangers,
            IMenuMarkupBuilder markupBuilder)
        {
            _menuChangers = menuChangers;
            _markupBuilder = markupBuilder;
        }

        public string BuildMarkup()
        {
            var requests = BuildChanges();
            return _markupBuilder.Build(requests);
        }
        private IEnumerable<MenuItem> BuildChanges()
        {
            var menuRequests = _menuChangers.Select(x => new
            {
                StartMenu = new MenuBuilder(), 
                MenuConfigurer = x.Configure()
            }).ToList();

            var menus = menuRequests.SelectMany(rq =>
            {
                rq.MenuConfigurer(rq.StartMenu);
                return rq.StartMenu.Build();
            });

            // TODO: merge menu requests

            return menus;
        }
    }
}