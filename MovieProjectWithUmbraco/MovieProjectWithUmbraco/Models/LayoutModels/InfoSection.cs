using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Models
{
    public class InfoSection
    {
        public IEnumerable<InfoItem> RecentMovies { get; set; }
        public IEnumerable<InfoItem> RecentPeople { get; set; }
        public IEnumerable<InfoItem> RecentDistributors { get; set; }
    }
}