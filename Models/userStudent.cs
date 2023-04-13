using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class userStudent
    {
        public int Student_ID { get; set; }
        public string Student_Name { get; set; }
        public string Student_Surname { get; set; }
        public string Student_Contact { get; set; }
        public string Student_Email { get; set; }
        public string Student_Password { get; set; }
        public string PhotoFileName { get; set; }
    }
}