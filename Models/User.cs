using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ScreenVault.Models
{
    public class User
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("username")]
        public required string Username { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("password")]
        public required string Password { get; set; }

        [BsonElement("roles")]
        public required List<string> Roles { get; set; }
    }
}