using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Models
{
    public class Layout
    {
        public IEnumerable<NavigationListItem> Links { get; set; }
        public string UserImage { get; set; }
        public string UserName { get; set; }
    }
}