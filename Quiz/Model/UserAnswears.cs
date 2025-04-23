using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    public class UserAnswears
    {
        public Question Question { get; set; }
        public List<Answear> CheckedAnswears { get; set; } = new List<Answear>();
        
    }
}
