using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Web_API.Controllers
{
    public class tutorController : ApiController
    {
        // Fetching information from the sql database using GET method.
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password, Module_Name, PhotoFileName
                   from dbo.Tutor
                   ";

            // Creating a data table to store information coming from the database table.
            DataTable tutor_table = new DataTable();

            using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            using (var sql_command = new SqlCommand(_query, sql_connection))
            using (var data_adapter = new SqlDataAdapter(sql_command))
            {
                sql_command.CommandType = CommandType.Text;
                data_adapter.Fill(tutor_table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tutor_table);
        }

        // Post information using POST method.
        public string Post(userTutor user_tutor)
        {
            try
            {
                string _query = @"
                       insert into dbo.Tutor values
                       (
                            '" + user_tutor.Tutor_Name + @"',
                            '" + user_tutor.Tutor_Surname + @"',
                            '" + user_tutor.Tutor_Contact + @"',
                            '" + user_tutor.Tutor_Email + @"',
                            '" + user_tutor.Tutor_Password + @"',
                            '" + user_tutor.Module_Name + @"',
                            '" + user_tutor.PhotoFileName + @"'
                       )";

                DataTable tutor_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(tutor_table);
                }

                return "Added tutor information successfully.";
            }
            catch (Exception)
            {
                return "Failed to add tutor information.";
            }
        }

        // Update information using the PUT method.
        public string Put(userTutor user_tutor)
        {
            try
            {
                string _query = @"
                       update dbo.Tutor set
                       Tutor_Name='" + user_tutor.Tutor_Name + @"'
                       ,Tutor_Surname='" + user_tutor.Tutor_Surname + @"'
                       ,Tutor_Contact='" + user_tutor.Tutor_Contact + @"'
                       ,Tutor_Email='" + user_tutor.Tutor_Email + @"'
                       ,Tutor_Password='" + user_tutor.Tutor_Password + @"'
                       ,Module_Name='" + user_tutor.Module_Name + @"'
                       ,PhotoFileName='" + user_tutor.PhotoFileName + @"'
                       where Tutor_ID=" + user_tutor.Tutor_ID + @"
                       ";

                DataTable tutor_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(tutor_table);
                }

                return "Updated tutor information successfully.";
            }
            catch (Exception)
            {
                return "Failed to update tutor information.";
            }
        }

        // Delete any information from sql database using DELETE method.
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Tutor
                       where Tutor_ID=" + id + @"
                       ";

                DataTable tutor_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(tutor_table);
                }

                return "Deleted tutor information successfully.";
            }
            catch (Exception)
            {
                return "Failed to delete tutor information.";
            }
        }

        //custome method to return all student names
        [Route("api/Student/GetAllStudentNames")]
        [HttpGet]
        public HttpResponseMessage GetAllStudentNames()
        {
            string _query = @"
                       select Student_Name from dbo.Student";

            //Creating a Data Table to store information coming from database table
            DataTable _table = new DataTable();

            using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            using (var sql_command = new SqlCommand(_query, sql_connection))
            using (var data_adapter = new SqlDataAdapter(sql_command))
            {
                sql_command.CommandType = CommandType.Text;
                data_adapter.Fill(_table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, _table);
        }

        [Route("api/Tutor/saveFile")]
        public string saveFile()
        {
            try
            {
                // to capture the current request
                var http_request = HttpContext.Current.Request;
                // only considering the first file upload in case multiple files are attached in the request 
                var posted_file = http_request.Files[0];
                string file_name = posted_file.FileName;
                // save in the photos folder
                var physical_path = HttpContext.Current.Server.MapPath("~/Photos/" + file_name);

                posted_file.SaveAs(physical_path);

                return file_name;
            }
            catch (Exception)
            {
                return "anonymous.jpg";
            }
        }

        [Route("api/tutor/checktutorprofile")]
        [HttpGet]
        public HttpResponseMessage getTutorProfile(String email)
        {
            List<userTutor> profile = new List<userTutor>();
            List<userTutor> theProfile = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password, PhotoFileName from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor prof = new userTutor();
                    prof.Tutor_Name = reader["Tutor_Name"].ToString();
                    prof.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    prof.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    prof.Tutor_Email = reader["Tutor_Email"].ToString();
                    prof.Tutor_Password = reader["Tutor_Password"].ToString();
                    prof.PhotoFileName = reader["PhotoFileName"].ToString();
                    profile.Add(prof);
                }
                foreach (userTutor theProf in profile)
                {
                    if (theProf.Tutor_Email == email)
                    {
                        isFound = true;
                        theProfile.Add(theProf);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theProfile);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Tutor Records Found");
                }
            }
        }

        [Route("api/tutor/checkregisteredmodules")]
        [HttpGet]
        public HttpResponseMessage getRegisteredModules(String email)
        {
            List<userTutor> modules = new List<userTutor>();
            List<userTutor> theModule = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password, Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor prof = new userTutor();
                    prof.Tutor_Name = reader["Tutor_Name"].ToString();
                    prof.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    prof.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    prof.Tutor_Email = reader["Tutor_Email"].ToString();
                    prof.Tutor_Password = reader["Tutor_Password"].ToString();
                    prof.Module_Name = reader["Module_Name"].ToString();
                    modules.Add(prof);
                }
                foreach (userTutor theProf in modules)
                {
                    if (theProf.Tutor_Email == email)
                    {
                        isFound = true;
                        theModule.Add(theProf);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theModule);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Tutor Records Found");
                }
            }
        }

        [Route("api/tutor/checktutorsession")]
        [HttpGet]
        public HttpResponseMessage getTutorSessions(String email)
        {
            List<session> _session = new List<session>();
            List<session> theSession = new List<session>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Session_ID, Module_Name, Tutor_Email, Student_Email, Session_Date, Start_Time, End_Time, Session_Status from dbo.Session", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    session sess = new session();
                    sess.Module_Name = reader["Module_Name"].ToString();
                    sess.Tutor_Email = reader["Tutor_Email"].ToString();
                    sess.Student_Email = reader["Student_Email"].ToString();
                    sess.Session_Date = reader["Session_Date"].ToString();
                    sess.Start_Time = reader["Start_Time"].ToString();
                    sess.End_Time = reader["End_Time"].ToString();
                    sess.Session_Status = reader["Session_Status"].ToString();
                    _session.Add(sess);
                }
                foreach (session theSess in _session)
                {
                    if (theSess.Tutor_Email == email)
                    {
                        isFound = true;
                        theSession.Add(theSess);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theSession);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Session Records Found");
                }
            }
        }

        [Route("api/tutor/countTutors")]
        [HttpGet]
        public HttpResponseMessage CountNumTutors() ///Count Total Number of Tutors
        {
            List<userTutor> Tutors = new List<userTutor>();
            int counter = 0;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor tutor in Tutors)
                {
                    counter++;
                }


            }
            return Request.CreateResponse(HttpStatusCode.OK, counter);
        }

        [Route("api/Tutor/checktutors")]
        [HttpGet]
        public HttpResponseMessage getTutorByModule(string module)
        {
            List<userTutor> Tutors = new List<userTutor>();
            List<userTutor> TutorsByModule = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (module == TheTutor.Module_Name)
                    {
                        isFound = true;
                        TutorsByModule.Add(TheTutor);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TutorsByModule);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Could Not Find Tutors For " + module);
                }
            }
        }

        [Route("api/Tutor/getSingleTutor")]
        [HttpGet]
        public HttpResponseMessage getTutorByEmail(string email)
        {
            List<userTutor> Tutors = new List<userTutor>();
            List<userTutor> TutorsByEmail = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (email == TheTutor.Tutor_Email)
                    {
                        isFound = true;
                        TutorsByEmail.Add(TheTutor);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TutorsByEmail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Such Tutor In Database");
                }
            }
        }

        [Route("api/Tutor/checktutors")]
        [HttpGet]
        public HttpResponseMessage getTutoByModule(string module)
        {
            List<userTutor> Tutors = new List<userTutor>();
            List<userTutor> TutorsByModule = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (module == TheTutor.Module_Name)
                    {
                        isFound = true;
                        TutorsByModule.Add(TheTutor);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TutorsByModule);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Could Not Find Tutors For " + module);
                }
            }
        }
        [Route("api/Tutors/countTutors")]
        [HttpGet]
        public HttpResponseMessage CountNumTutos() ///Count Total Number of Tutors
        {
            List<userTutor> Tutors = new List<userTutor>();
            int counter = 0;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor tutor in Tutors)
                {
                    counter++;
                }


            }
            return Request.CreateResponse(HttpStatusCode.OK, counter);
        }
        [Route("api/Tutor/getSingleTutor")]
        [HttpGet]
        public HttpResponseMessage getTutoByEmail(string email)
        {
            List<userTutor> Tutors = new List<userTutor>();
            List<userTutor> TutorsByEmail = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (email == TheTutor.Tutor_Email)
                    {
                        isFound = true;
                        TutorsByEmail.Add(TheTutor);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TutorsByEmail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Such Tutor In Database");
                }
            }
        }
        [Route("api/Tutor/getTutorByModule")]
        [HttpGet]
        public HttpResponseMessage getTutorByMod(string module)
        {
            List<userTutor> Tutors = new List<userTutor>();
            List<userTutor> TutorsByEmail = new List<userTutor>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();
                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (module == TheTutor.Module_Name)
                    {
                        isFound = true;
                        TutorsByEmail.Add(TheTutor);
                        break;
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TutorsByEmail);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Such Tutor In Database");
                }
            }
        }
        [Route("api/tutor/getTutByMod")]
        [HttpGet]
        public HttpResponseMessage getTutByMod(string mod)
        {
            List<userTutor> Tutors = new List<userTutor>();
            userTutor SingleTutor = new userTutor();
            bool isFound = false;
            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Tutor_ID, Tutor_Name, Tutor_Surname, Tutor_Contact, Tutor_Email, Tutor_Password,Module_Name from dbo.Tutor", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userTutor tutor = new userTutor();

                    tutor.Tutor_Name = reader["Tutor_Name"].ToString();
                    tutor.Tutor_Surname = reader["Tutor_Surname"].ToString();
                    tutor.Tutor_Contact = reader["Tutor_Contact"].ToString();
                    tutor.Tutor_Email = reader["Tutor_Email"].ToString();
                    tutor.Tutor_Password = reader["Tutor_Password"].ToString();
                    tutor.Module_Name = reader["Module_Name"].ToString();
                    Tutors.Add(tutor);
                }
                foreach (userTutor TheTutor in Tutors)
                {
                    if (mod == TheTutor.Module_Name)
                    {
                        isFound = true;

                        SingleTutor.Tutor_Name = TheTutor.Tutor_Name;
                        SingleTutor.Tutor_Surname = TheTutor.Tutor_Surname;
                        SingleTutor.Tutor_Contact = TheTutor.Tutor_Contact;
                        SingleTutor.Tutor_Email = TheTutor.Tutor_Email;
                        SingleTutor.Tutor_Password = TheTutor.Tutor_Password;
                        SingleTutor.Module_Name = TheTutor.Module_Name;
                        break;
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, SingleTutor);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Such Tutor In Database");
                }
            }
        }

        [Route("api/tutor/getTheModuleName")]
        [HttpGet]
        public string getTheModuleName(string strUrl)
        {
            string[] tokens = strUrl.Split('/');
            string[] moduleName = tokens[4].Split('.');
            return moduleName[0];
        }
    }
}
