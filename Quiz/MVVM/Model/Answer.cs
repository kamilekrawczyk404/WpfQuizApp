using Quiz.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Answer : Core.ViewModel
    {

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
        private bool _isCorrect;
        public bool IsCorrect
        {
            get { return _isCorrect; }
            set
            {
                _isCorrect = value;
                OnPropertyChanged();
            }
        }
        
        public Answer()
        {
            Text = string.Empty;
            IsCorrect = false;
        }

        public Answer(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public bool Equals(Answer other)
        {
            if (other == null) return false;
            return Text == other.Text && IsCorrect == other.IsCorrect;
        }

        public override string ToString()
        {
            return "Answer - " + Text + ", IsCorrect: " + (IsCorrect == true ? "yes" : "no");
        }
    }
}
