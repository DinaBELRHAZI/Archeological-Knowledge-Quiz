
using ApiQuiz.Models;
using AppMobile;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ApiQuiz.Services
{
    public class QuizService
    {

        private readonly IMongoCollection<quiz> _quizCollection;

        public QuizService(
            IOptions<QuizStoreDatabaseSettings> QuizStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                QuizStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                QuizStoreDatabaseSettings.Value.DatabaseName);

            _quizCollection = mongoDatabase.GetCollection<quiz>(
                QuizStoreDatabaseSettings.Value.QuizCollectionName);
        }

        public async Task<List<quiz>> GetAsync() =>
        await _quizCollection.Find(_ => true).ToListAsync();

        public async Task<quiz?> GetAsync(string id) =>
            await _quizCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(quiz newQuiz) =>
            await _quizCollection.InsertOneAsync(newQuiz);

        public async Task UpdateAsync(string id, quiz updatedQuiz) =>
            await _quizCollection.ReplaceOneAsync(x => x.Id == id, updatedQuiz);

        public async Task RemoveAsync(string id) =>
            await _quizCollection.DeleteOneAsync(x => x.Id == id);


    }
}
