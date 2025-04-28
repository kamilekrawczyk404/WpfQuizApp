using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class UserAnswers
    {
        public Question Question { get; set; }
        public List<Answer> CheckedAnswears { get; set; } = new List<Answer>();
        
    }
}
