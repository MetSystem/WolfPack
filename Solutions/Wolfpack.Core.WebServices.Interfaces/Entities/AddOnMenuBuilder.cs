
using System.Collections.Generic;
using System.Linq;

namespace Wolfpack.Core.WebServices.Interfaces.Entities
{
    //public class Test : INeedMenuSpace
    //{
    //    public Action<IMenuBuilder> Configure()
    //    {
    //        return menu => menu
    //            .AddDropdown("Sub-Drop")
    //            .AddItem("Item1", "/dashboard")
    //            .AddDivider()
    //            .AddItem("Item2", "/ui/item2")
    //            .Up()
    //            .AddItem("Item3", "/ui/item3");
    //    }
    //}

    public class AddOnMenuBuilder : IMenuBuilder
    {
        private readonly IList<IMenuItemBuilder> _builders;

        public AddOnMenuBuilder()
        {
            _builders = new List<IMenuItemBuilder>();
        }

        IDropDownMenuBuilder<IMenuBuilder> IMenuBuilder.AddDropdown(string name)
        {
            var dropdownBuilder = new DropDownMenuItemBuilder<IMenuBuilder>(this, name);
            _builders.Add(dropdownBuilder);
            return dropdownBuilder;
        }

        public IMenuBuilder AddItem(string name, string url, string target = null)
        {
            _builders.Add(new MenuItemBuilder(name, url, target));
            return this;
        }

        public IMenuBuilder AddDivider()
        {
            _builders.Add(new PassThruMenuItemBuilder(MenuItem.Divider()));
            return this;
        }

        public IEnumerable<MenuItem> Build()
        {
            var addonDropdownBuilder = ((IDropDownMenuBuilder<IMenuBuilder>)
                new DropDownMenuItemBuilder<IMenuBuilder>(this, "Add-Ons"))
                .AddItems(_builders.SelectMany(x => x.Build()));

            return ((IMenuItemBuilder)addonDropdownBuilder).Build().ToList();
        }
    }
}