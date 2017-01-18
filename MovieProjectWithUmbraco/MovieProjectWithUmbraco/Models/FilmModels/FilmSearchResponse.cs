using System;

namespace MovieProjectWithUmbraco.Models
{
    public class FilmSearchResponse
    {
        public string OrderBy { get; set; }
        public string Query { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Rating { get; set; }
    }
}