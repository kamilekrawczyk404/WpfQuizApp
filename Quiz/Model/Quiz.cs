using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Quiz
    {
        public string Title { get; set; }
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public TimeSpan Duration { get; set; }

        public int CalculateScore(List<UserAnswears> userAnswears)
        {
            int score = 0;
            foreach(var userAnswear in userAnswears)
            {
                if (userAnswear.Question.CheckAnswear(userAnswear.CheckedAnswears))
                {
                    score += 1;
                }
            }
            return score;
        }

        public int MaxScore()
        {
            return Questions.Sum(x => x.Points);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
