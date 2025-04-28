using Quiz.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Question : Core.ViewModel
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        private QuestionType _type;
        public QuestionType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }
        private int _points;
        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Answer> Answers { get; set; }

        public Question()
        {
            Title = string.Empty;
            Type = QuestionType.SingleCorrect;
            Points = 0;
            Answers = new ObservableCollection<Answer>(new Answer[4] {new Answer(), new Answer(), new Answer(), new Answer()});
        }

        public Question(string title, QuestionType type, int points, ObservableCollection<Answer> answers)
        {
            Title = title;
            Type = type;
            Points = points;
            Answers = answers;
        }

        public bool CheckAnswear(List<Answer> checkedAnswers)
        {
            if (Type == QuestionType.SingleCorrect)
            {
                // single choice
                if (checkedAnswers.Count != 1)
                {
                    return false;
                }

                return Answers.FirstOrDefault(x => x.IsCorrect)?.Equals(checkedAnswers.Single()) ?? false;
            }
            else
            {
                // multiple choice
                var correctAnswers = Answers.Where(x => x.IsCorrect).ToList();
                if (correctAnswers.Count != checkedAnswers.Count)
                {
                    return false;
                }
                return correctAnswers.All(x => checkedAnswers.Contains(x));
            }
        }
    }
}
