using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MovieDatabase.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("release_year")]
        public int ReleaseYear { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("cast")]
        public List<CastMember> Cast { get; set; }

        [BsonElement("poster_url")]
        public string PosterUrl { get; set; }

        [BsonElement("trailer_url")]
        public string TrailerUrl { get; set; }
    }

    public class CastMember
    {
        [BsonElement("actor_name")]
        public string ActorName { get; set; }

        [BsonElement("character_name")]
        public string CharacterName { get; set; }
    }
}