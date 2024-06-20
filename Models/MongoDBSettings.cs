namespace MovieDatabase.Models
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}