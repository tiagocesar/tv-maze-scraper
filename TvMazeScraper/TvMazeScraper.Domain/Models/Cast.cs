using MongoDB.Bson.Serialization.Attributes;

namespace TvMazeScraper.Domain.Models
{
    public class Cast
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("birthday")]
        public string Birthday { get; set; }
    }
}