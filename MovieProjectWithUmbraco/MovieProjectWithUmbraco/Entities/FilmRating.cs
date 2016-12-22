namespace MovieProjectWithUmbraco.Entities
{
    public class FilmRating
    {
        public long Id { get; set; }
        public double Rating { get; set; }
        public long UserId { get; set; }
        public long FilmId { get; set; }
    }
}