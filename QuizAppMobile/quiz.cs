using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace QuizAppMobile
{
    public class quiz
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("id_quiz")]
        public int? id_quiz { get; set; }

        [BsonElement("question")]
        public string? question { get; set; }

        [BsonElement("reponse_1")]
        public string? reponse_1 { get; set; }

        [BsonElement("reponse_2")]
        public string? reponse_2 { get; set; }

        [BsonElement("reponse_3")]
        public string? reponse_3 { get; set; }

        [BsonElement("reponse_4")]
        public string? reponse_4 { get; set; }

        [BsonElement("bonne_reponse")]
        public int? bonne_reponse { get; set; }

        [BsonElement("description")]
        public string? description { get; set; }
    }
}