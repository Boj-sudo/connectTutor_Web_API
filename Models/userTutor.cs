using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class userTutor
    {
        public int Tutor_ID { get; set; }
        public string Tutor_Name { get; set; }
        public string Tutor_Surname { get; set; }
        public string Tutor_Contact { get; set; }
        public string Tutor_Email { get; set; }
        public string Tutor_Password { get; set; }
        public string PhotoFileName { get; set; }
        public string Module_Name { get; set; }
    }
}