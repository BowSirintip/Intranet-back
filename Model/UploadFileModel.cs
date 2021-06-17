using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestAPIDemo.Model
{
    public class UploadFileModel
    {
        public int Id { get; set; }
        public IFormFile files { get; set; }
        public string Name { get; set; }
        public string subject { get; set; }
        public string imgPath { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
    }
}
