using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Model
{
    public class UploadFileMenuModel
    {
        public int Id { get; set; }
        public string subject { get; set; }
        public string URLPath { get; set; }
        public string filePath { get; set; }
        public int create_by { get; set; }
    }
}
