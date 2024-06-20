using MongoDB.Driver;
using ScreenVault.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScreenVault.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _movies = database.GetCollection<Movie>("movies");
        }

        public List<Movie> Get() => _movies.Find(movie => true).ToList();

        public Movie Get(int id) => _movies.Find<Movie>(movie => movie.Id == id).FirstOrDefault();

        public async Task<Movie> Create(Movie movie)
        {
            movie.Id = await GetNextSequenceValue();
            await _movies.InsertOneAsync(movie);
            return movie;
        }

        public void Update(int id, Movie movieIn) => _movies.ReplaceOne(movie => movie.Id == id, movieIn);

        public void Remove(int id) => _movies.DeleteOne(movie => movie.Id == id);

        private async Task<int> GetNextSequenceValue()
        {
            var sort = Builders<Movie>.Sort.Descending(m => m.Id);
            var lastMovie = await _movies.Find(movie => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return lastMovie != null ? lastMovie.Id + 1 : 1;
        }
    }
}