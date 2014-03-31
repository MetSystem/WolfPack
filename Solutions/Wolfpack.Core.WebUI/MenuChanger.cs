using System.Collections.Generic;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebUI
{
    public class MenuChanger
    {
        private readonly IEnumerable<INeedMenuSpace> _menuChangers;

        public MenuChanger(IEnumerable<INeedMenuSpace> menuChangers)
        {
            _menuChangers = menuChangers;
        }

        public MenuChanges BuildChanges()
        {
            return new MenuChanges();
        }
    }
}