using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Model
{
    public class NewsModel
    {
        public string HeadNew { get; set; }
        public string contentShort { get; set; }
        public string contentAll { get; set; }
        public string imgPath { get; set; }
        public int depID { get; set; }
        public string create_by { get; set; }
    }
}
