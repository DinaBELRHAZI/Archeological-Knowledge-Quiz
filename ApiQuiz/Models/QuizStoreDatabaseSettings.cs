namespace ApiQuiz.Models
{
    public class QuizStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string QuizCollectionName { get; set; } = null!;
    }
}
