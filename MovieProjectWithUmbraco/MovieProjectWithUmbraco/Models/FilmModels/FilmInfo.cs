using System;

namespace MovieProjectWithUmbraco.Models
{
    public class FilmInfo
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public DateTime YearOfRelease { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public double? TotalRating { get; set; }
        public double? PersonalRating { get; set; }
    }
}