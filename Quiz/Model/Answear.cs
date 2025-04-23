using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class Answear
    {
        public string AnswearText { get; set; }
        public bool IsCorrect { get; set; }

        public override string ToString()
        {
            return AnswearText;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Answear other = (Answear)obj;
            return AnswearText == other.AnswearText && IsCorrect == other.IsCorrect;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AnswearText, IsCorrect);
        }
    }
}
