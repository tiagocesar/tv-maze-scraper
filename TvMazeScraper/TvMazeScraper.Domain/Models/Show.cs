using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace TvMazeScraper.Domain.Models
{
    public class Show
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("cast")]
        public IList<Cast> Cast { get; set; }
    }
}