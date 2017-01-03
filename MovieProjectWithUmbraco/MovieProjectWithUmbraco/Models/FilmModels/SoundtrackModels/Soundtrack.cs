using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Models
{
    public class Soundtrack
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public IEnumerable<LinkResponse> Composer { get; set; }
    }
}