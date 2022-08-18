using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMobile
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
        

       /* public quiz(int id, string question, string reponse_1, string reponse_2, string reponse_3, string reponse_4, int bonne_reponse, string description)
        {
            this.id = id;
            this.question = question;
            this.reponse_1 = reponse_1;
            this.reponse_2 = reponse_2;
            this.reponse_3 = reponse_3;
            this.reponse_4 = reponse_4;
            this.bonne_reponse = bonne_reponse;
            this.description = description;
        }*/

        /*public quiz(string Id, int id_quiz, string question, string reponse_1, string reponse_2, string reponse_3, string reponse_4, int bonne_reponse, string description)
        {
            this.Id = Id;
            this.id_quiz = id_quiz;
            this.question = question;
            this.reponse_1 = reponse_1;
            this.reponse_2 = reponse_2;
            this.reponse_3 = reponse_3;
            this.reponse_4 = reponse_4;
            this.bonne_reponse = bonne_reponse;
            this.description = description;
        }*/

       /* public override string ToString()
        {
            return this.id_quiz + " " + this.question + " " + this.bonne_reponse + " " + this.description ;
        }*/
    }

}
