using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Model
{
    public class ImagesModel
    {
        public int Id { get; set; }
        public int topicType { get; set; }
        public string topicDetail { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
       /* public imgSub_Path { get; set; }*/
       public int[] idimg { get; set; }
    }
}
