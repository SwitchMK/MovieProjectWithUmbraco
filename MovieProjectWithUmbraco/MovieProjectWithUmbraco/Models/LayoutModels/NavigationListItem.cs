using System.Collections.Generic;
using System.Linq;

namespace MovieProjectWithUmbraco.Models
{
    public class NavigationListItem
    {
        public string Text { get; set; }
        public NavigationLink Link { get; set; }
        public IEnumerable<NavigationListItem> Items { get; set; }
        public bool HasChildren => Items != null && Items.Any() && Items.Any();

        public NavigationListItem() { }

        public NavigationListItem(string text)
        {
            Text = text;
        }

        public NavigationListItem(NavigationLink link)
        {
            Link = link;
        }
    }
}