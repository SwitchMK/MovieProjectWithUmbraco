using System;

namespace MovieProjectWithUmbraco.Models
{
    public class FilmFilter
    {
        public FilmOrderType[] OrderBy { get; set; }
        public string Query { get; set; }
        public string Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class FilmOrderType
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    }
}