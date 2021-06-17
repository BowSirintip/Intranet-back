using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIDemo.Model
{
    public class UploadKMModel
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string detail { get; set; }
        public string pdfPath { get; set; }
        public string videoPath { get; set; }
        public int create_by { get; set; }
        public int dep_id { get; set; }
    }
}
