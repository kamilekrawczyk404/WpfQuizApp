using Quiz.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class QuizModel : Core.ViewModel
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

        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();

        public TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public QuizModel()
        {
            Title = "";
            Questions = new ObservableCollection<Question>();
        }

        public QuizModel(string title, ObservableCollection<Question> questions)
        {
            Title = title;
            Questions = questions;
        }

        public int MaxScore()
        {
            return Questions.Sum(x => x.Points);
        }
    }
}
