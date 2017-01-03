using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Models
{
    public class SoundtracksList
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public int TotalDuration { get; set; }
        public IEnumerable<Soundtrack> Soundtracks { get; set; }
    }
}