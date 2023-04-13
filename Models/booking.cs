using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class booking
    {
        public string Student_Email { get; set; }
        public string Module_Name { get; set; }
        public string Tutor_Email { get; set; }
        public string Start_Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
    }
}