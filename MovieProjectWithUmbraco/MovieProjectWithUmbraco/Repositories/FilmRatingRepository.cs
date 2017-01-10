using MovieProjectWithUmbraco.Entities;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MovieProjectWithUmbraco.Repositories
{
    public class FilmRatingRepository : IFilmRatingRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<FilmRating> _filmRatingDataSet;

        public FilmRatingRepository(DbContext context)
        {
            _context = context;
            _filmRatingDataSet = context.Set<FilmRating>();
        }

        public IEnumerable<FilmRating> GetFilmRatings(long userId)
        {
            return _filmRatingDataSet.Where(p => p.UserId == userId);
        }

        public double? GetPersonalRating(long? filmId, long? userId)
        {
            var record = _filmRatingDataSet.FirstOrDefault(p => p.FilmId == filmId && p.UserId == userId);

            return record?.Rating;
        }

        public double? GetTotalRating(long filmId)
        {
            var records = _filmRatingDataSet
                .Where(p => p.FilmId == filmId);

            if (records.Count() == 0)
                return null;

            return records.Average(p => p.Rating);
        }

        public void AddRating(long filmId, long userId, double rating)
        {
            var filmRating = new FilmRating
            {
                FilmId = filmId,
                Rating = rating,
                UserId = userId
            };

            _filmRatingDataSet.Add(filmRating);

            _context.SaveChanges();
        }

        public void UpdateRating(long filmId, long userId, double rating)
        {
            var filmRating = _filmRatingDataSet.FirstOrDefault(p => p.FilmId == filmId && p.UserId == userId);
            filmRating.Rating = rating;

            _context.Entry(filmRating).State = EntityState.Modified;

            _context.SaveChanges();
        }
    }
}