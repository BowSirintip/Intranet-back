// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace RestAPIDemo
{
    public partial class menu
    {
        public int id { get; set; }
        public int? depID { get; set; }
        public string menuName { get; set; }
        public int? groupID { get; set; }
        public int? isDelete { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
    }
}