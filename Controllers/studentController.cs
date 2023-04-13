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
    public class studentController : ApiController
    {
        // Fetching information from the sql database using GET method.
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Student_ID, Student_Name, Student_Surname, Student_Contact, Student_Email, Student_Password, PhotoFileName
                   from dbo.Student
                   ";

            // Creating a data table to store information coming from the database table.
            DataTable student_table = new DataTable();

            using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            using (var sql_command = new SqlCommand(_query, sql_connection))
            using (var data_adapter = new SqlDataAdapter(sql_command))
            {
                sql_command.CommandType = CommandType.Text;
                data_adapter.Fill(student_table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, student_table);
        }

        // Post information using POST method.
        public string Post(userStudent user_student)
        {
            try
            {
                string _query = @"
                       insert into dbo.Student values
                       (
                            '" + user_student.Student_Name + @"',
                            '" + user_student.Student_Surname + @"',
                            '" + user_student.Student_Contact + @"',
                            '" + user_student.Student_Email + @"',
                            '" + user_student.Student_Password + @"',
                            '" + user_student.PhotoFileName + @"'
                       )";

                DataTable student_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(student_table);
                }

                return "Added student information successfully.";
            }
            catch (Exception)
            {
                return "Failed to add student information.";
            }
        }

        // Update information using the PUT method.
        public string Put(userStudent user_student)
        {
            try
            {
                string _query = @"
                       update dbo.Student set
                       Student_Name='" + user_student.Student_Name + @"'
                       ,Student_Surname='" + user_student.Student_Surname + @"'
                       ,Student_Contact='" + user_student.Student_Contact + @"'
                       ,Student_Email='" + user_student.Student_Email + @"'
                       ,Student_Password='" + user_student.Student_Password + @"'
                       ,PhotoFileName='" + user_student.PhotoFileName + @"'
                       where Student_ID=" + user_student.Student_ID + @"
                       ";

                DataTable student_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(student_table);
                }

                return "Updated student information successfully.";
            }
            catch (Exception)
            {
                return "Failed to update student information.";
            }
        }

        // Delete any information from sql database using DELETE method.
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Student
                       where Student_ID=" + id + @"
                       ";

                DataTable student_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(student_table);
                }

                return "Deleted student information successfully.";
            }
            catch (Exception)
            {
                return "Failed to delete student information.";
            }
        }

        //custome method to return all tutor names
        [Route("api/Tutor/GetAllTutorNames")]
        [HttpGet]
        public HttpResponseMessage GetAllTutorNames()
        {
            string _query = @"
                       select Tutor_Name from dbo.Tutor";

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

        [Route("api/Student/saveFile")]
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

        [Route("api/student/checkstudentprofile")]
        [HttpGet]
        public HttpResponseMessage getStudentProfile(String email)
        {
            List<userStudent> profile = new List<userStudent>();
            List<userStudent> theProfile = new List<userStudent>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Student_ID, Student_Name, Student_Surname, Student_Contact, Student_Email, Student_Password, PhotoFileName from dbo.Student", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userStudent prof = new userStudent();
                    prof.Student_Name = reader["Student_Name"].ToString();
                    prof.Student_Surname = reader["Student_Surname"].ToString();
                    prof.Student_Contact = reader["Student_Contact"].ToString();
                    prof.Student_Email = reader["Student_Email"].ToString();
                    prof.Student_Password = reader["Student_Password"].ToString();
                    prof.PhotoFileName = reader["PhotoFileName"].ToString();
                    profile.Add(prof);
                }
                foreach (userStudent theProf in profile)
                {
                    if (theProf.Student_Email == email)
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
                    return Request.CreateResponse(HttpStatusCode.OK, "No Student Records Found");
                }
            }
        }

        [Route("api/Student/countStudents")]
        [HttpGet]
        public HttpResponseMessage CountNumStudents() ///Count Total Number of Tutors
        {
            List<userStudent> Students = new List<userStudent>();
            int counter = 0;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Student_ID, Student_Name, Student_Surname, Student_Contact, Student_Email, Student_Password from dbo.Student", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userStudent student = new userStudent();
                    student.Student_Name = reader["Student_Name"].ToString();
                    student.Student_Surname = reader["Student_Surname"].ToString();
                    student.Student_Contact = reader["Student_Contact"].ToString();
                    student.Student_Email = reader["Student_Email"].ToString();
                    student.Student_Password = reader["Student_Password"].ToString();
                    Students.Add(student);
                }
                foreach (userStudent std in Students)
                {
                    counter++;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, counter);
        }

        [Route("api/student/checkstudentsession")]
        [HttpGet]
        public HttpResponseMessage getStudentSessions(String email)
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
                    if (theSess.Student_Email == email)
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

        [Route("api/Student/getSingleStudent")]
        [HttpGet]
        public HttpResponseMessage getStudentByEmail(string email) ///Count Total Number of Tutors
        {
            List<userStudent> Students = new List<userStudent>();
            List<userStudent> TheStudents = new List<userStudent>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Student_ID, Student_Name, Student_Surname, Student_Contact, Student_Email, Student_Password from dbo.Student", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userStudent student = new userStudent();
                    student.Student_Name = reader["Student_Name"].ToString();
                    student.Student_Surname = reader["Student_Surname"].ToString();
                    student.Student_Contact = reader["Student_Contact"].ToString();
                    student.Student_Email = reader["Student_Email"].ToString();
                    student.Student_Password = reader["Student_Password"].ToString();
                    Students.Add(student);
                }
                foreach (userStudent std in Students)
                {
                    if (std.Student_Email == email)
                    {
                        isFound = true;
                        TheStudents.Add(std);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheStudents);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Such Student In Database");
                }
            }

        }
    }
}
