using System;

namespace MovieProjectWithUmbraco.Models
{
    public class UserComment
    {
        public string Publisher { get; set; }
        public string PublisherProfileUrl { get; set; }
        public int MemberId { get; set; }
        public string Content { get; set; }
        public string FilmName { get; set; }
        public string FilmPageUrl { get; set; }
        public DateTime DateOfPublication { get; set; }
    }
}