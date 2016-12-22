namespace MovieProjectWithUmbraco.Models.Requests
{
    public class RateRequest
    {
        public long FilmId { get; set; }
        public double Rating { get; set; }
    }
}