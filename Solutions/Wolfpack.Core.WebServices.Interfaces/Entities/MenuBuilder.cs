
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wolfpack.Core.WebServices.Interfaces.Entities
{
    public class Test : INeedMenuSpace
    {
        public Action<IMenuBuilder> Configure()
        {
            return menu => menu.AddDropdown("Add-ons")
                .AddItem("Item1", "/dashboard")
                .AddDivider()
                .AddItem("Item2", "/ui/item2");
        }
    }

    public class MenuBuilder : IMenuBuilder
    {
        private readonly IList<IMenuItemBuilder> _builders;

        public MenuBuilder()
        {
            _builders = new List<IMenuItemBuilder>();
        }

        IDropDownMenuBuilder<IMenuBuilder> IMenuBuilder.AddDropdown(string name)
        {
            var dropdown = new DropDownMenuBuilder<IMenuBuilder>(this, name);
            _builders.Add(dropdown);
            return dropdown;
        }

        public IEnumerable<MenuItem> Build()
        {
            return _builders.SelectMany(b => b.Build());
        }

    }

    public interface IMenuBuilder
    {
        IDropDownMenuBuilder<IMenuBuilder> AddDropdown(string name);
    }

    public interface IMenuItemBuilder
    {
        IEnumerable<MenuItem> Build();
    }


    public class MenuItem
    {
        private readonly List<MenuItem> _children;

        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public IList<MenuItem> Children { get { return _children; }}

        public MenuItem()
        {
            _children = new List<MenuItem>();
        }

        public static MenuItem Item(string name, string url, string target = null)
        {
            return new MenuItem
            {
                Name = name,
                Type = "Item",
                Url = url,
                Target = target
            };
        }
        public static MenuItem Dropdown(string name)
        {
            return new MenuItem
            {
                Name = name,
                Type = "DropDown"
            };
        }

        public static MenuItem Divider()
        {
            return new MenuItem
            {
                Type = "Divider"
            };
        }
    }

    public class DropDownMenuBuilder<TOwner> : IDropDownMenuBuilder<TOwner>
    {
        private readonly TOwner _parent;
        private readonly MenuItem _item;

        public DropDownMenuBuilder(TOwner parent, string name)
        {
            _parent = parent;
            _item = MenuItem.Dropdown(name);
        }

        IDropDownMenuBuilder<TOwner> IDropDownMenuBuilder<TOwner>.AddItem(string name, string url)
        {
            _item.Children.Add(MenuItem.Item(name, url));
            return this;
        }

        public IDropDownMenuBuilder<TOwner> AddItem(string name, string url, string target)
        {
            _item.Children.Add(MenuItem.Item(name, url, target));
            return this;
        }

        IDropDownMenuBuilder<TOwner> IDropDownMenuBuilder<TOwner>.AddDivider()
        {
            _item.Children.Add(MenuItem.Divider());
            return this;
        }

        TOwner IDropDownMenuBuilder<TOwner>.Up()
        {
            return _parent;
        }

        public IEnumerable<MenuItem> Build()
        {
            return new[] {_item};
        }
    }
        
    public interface IDropDownMenuBuilder<TOwner>  : IMenuItemBuilder
    {
        IDropDownMenuBuilder<TOwner> AddItem(string name, string url);
        IDropDownMenuBuilder<TOwner> AddItem(string name, string url, string target);
        IDropDownMenuBuilder<TOwner> AddDivider();
        TOwner Up();
    }
}