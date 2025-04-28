using Quiz.Core;
using Quiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Stores
{
    public class QuizStore
    {
        public Question CurrentQuestion { get; set; }
        public QuizModel CurrentQuiz { get; set; } = new QuizModel();
        public List<QuizModel> Quizzes { get; set; } = new List<QuizModel>();

        public event Action<Question> QuestionCreated;
        public event Action<QuizModel> QuizCreated;
        public event Action<Question> QuestionEdited;
        public void CreateQuestion(Question question)
        {
            QuestionCreated?.Invoke(question);
        }
        public void EditQuestion(Question question)
        {
            QuestionEdited?.Invoke(question);
        }
        public void CreateQuiz(QuizModel quiz)
        {
            QuizCreated?.Invoke(quiz);
        }
    }
}
