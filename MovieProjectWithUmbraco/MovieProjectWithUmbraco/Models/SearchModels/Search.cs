namespace MovieProjectWithUmbraco.Models
{
    public class Search
    {
        public string Query { get; set; }
        public string[] Types { get; set; }
        public string OrderBy { get; set; }
    }
}