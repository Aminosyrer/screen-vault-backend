using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MovieDatabase.Models
{
    public class Movie
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("title")]
        public required string Title { get; set; }

        [BsonElement("description")]
        public required string Description { get; set; }

        [BsonElement("release_year")]
        public int ReleaseYear { get; set; }

        [BsonElement("genres")]
        public required List<string> Genres { get; set; }

        [BsonElement("cast")]
        public required List<CastMember> Cast { get; set; }

        [BsonElement("poster_url")]
        public string? PosterUrl { get; set; }

        [BsonElement("trailer_url")]
        public string? TrailerUrl { get; set; }
    }

    public class CastMember
    {
        [BsonElement("actor_name")]
        public required string ActorName { get; set; }

        [BsonElement("character_name")]
        public required string CharacterName { get; set; }
    }
}