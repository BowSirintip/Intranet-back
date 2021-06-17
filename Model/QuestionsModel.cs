using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Model
{
    public class QuestionsModel
    {
        public int ansid { get; set; }
        public string topic { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int create_by { get; set; }
        public int update_by { get; set; }
        public int depid { get; set; }
    }
}
