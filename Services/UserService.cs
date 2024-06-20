using MongoDB.Driver;
using ScreenVault.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScreenVault.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("users");
        }

        public List<User> Get() => _users.Find(user => true).ToList();

        public User Get(int id) => _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByUsername(string username) => _users.Find<User>(user => user.Username == username).FirstOrDefault();

        public async Task<User> Create(User user)
        {
            user.Id = await GetNextSequenceValue();
            await _users.InsertOneAsync(user);
            return user;
        }

        public void Update(int id, User userIn) => _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(int id) => _users.DeleteOne(user => user.Id == id);

        private async Task<int> GetNextSequenceValue()
        {
            var sort = Builders<User>.Sort.Descending(u => u.Id);
            var lastUser = await _users.Find(user => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return lastUser != null ? lastUser.Id + 1 : 1;
        }
    }
}