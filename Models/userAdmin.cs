using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class userAdmin
    {
        public int Admin_ID { get; set; }
        public string Admin_Name { get; set; }
        public string Admin_Surname { get; set; }
        public string Admin_Contact { get; set; }
        public string Admin_Email { get; set; }
        public string Admin_Password { get; set; }
        public string PhotoFileName { get; set; }
    }
}