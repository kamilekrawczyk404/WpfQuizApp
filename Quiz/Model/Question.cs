using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Question
    {
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public int Points { get; set; } = 1;

        public ObservableCollection<Answear> Answears { get; set; } = new ObservableCollection<Answear>();

        public bool CheckAnswear(List<Answear> checkedAnswears)
        {
            if (Type == QuestionType.SingleChoice)
            {
                // single choice
                if (checkedAnswears.Count != 1)
                {
                    return false;
                }

                return Answears.FirstOrDefault(x => x.IsCorrect)?.Equals(checkedAnswears.Single()) ?? false;
            } 
            else 
            {
                // multiple choice
                var correctAnswears = Answears.Where(x => x.IsCorrect).ToList();
                if (correctAnswears.Count != checkedAnswears.Count)
                {
                    return false;
                }
                return correctAnswears.All(x => checkedAnswears.Contains(x));
            }
        }

        public override string ToString()
        {
            return QuestionText;
        }
    }
}
