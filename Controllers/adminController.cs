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
    public class adminController : ApiController
    {
        // Fetching information from the sql database using GET method.
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Admin_ID, Admin_Name, Admin_Surname, Admin_Contact, Admin_Email, Admin_Password, PhotoFileName
                   from dbo.Admin
                   ";

            // Creating a data table to store information coming from the database table.
            DataTable admin_table = new DataTable();

            using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            using (var sql_command = new SqlCommand(_query, sql_connection))
            using (var data_adapter = new SqlDataAdapter(sql_command))
            {
                sql_command.CommandType = CommandType.Text;
                data_adapter.Fill(admin_table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, admin_table);
        }

        // Post information using POST method.
        public string Post(userAdmin user_admin)
        {
            try
            {
                string _query = @"
                       insert into dbo.Admin values
                       (
                            '" + user_admin.Admin_Name + @"',
                            '" + user_admin.Admin_Surname + @"',
                            '" + user_admin.Admin_Contact + @"',
                            '" + user_admin.Admin_Email + @"',
                            '" + user_admin.Admin_Password + @"',
                            '" + user_admin.PhotoFileName + @"'
                       )";

                DataTable admin_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(admin_table);
                }

                return "Added admin information successfully.";
            }
            catch (Exception)
            {
                return "Failed to add admin information.";
            }
        }

        // Update information using the PUT method.
        public string Put(userAdmin user_admin)
        {
            try
            {
                string _query = @"
                       update dbo.Admin set
                       Admin_Name='" + user_admin.Admin_Name + @"'
                       ,Admin_Surname='" + user_admin.Admin_Surname + @"'
                       ,Admin_Contact='" + user_admin.Admin_Contact + @"'
                       ,Admin_Email='" + user_admin.Admin_Email + @"'
                       ,Admin_Password='" + user_admin.Admin_Password + @"'
                       ,PhotoFileName='" + user_admin.PhotoFileName + @"'
                       where Admin_ID=" + user_admin.Admin_ID + @"
                       ";

                DataTable admin_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(admin_table);
                }

                return "Updated admin information successfully.";
            }
            catch (Exception)
            {
                return "Failed to update admin information.";
            }
        }

        // Delete any information from sql database using DELETE method.
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Admin
                       where Admin_ID=" + id + @"
                       ";

                DataTable admin_table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(admin_table);
                }

                return "Deleted admin information successfully.";
            }
            catch (Exception)
            {
                return "Failed to delete admin information.";
            }
        }

        //custome method to return all student names
        [Route("api/Admin/GetAllAdminNames")]
        [HttpGet]
        public HttpResponseMessage GetAllAdminNames()
        {
            string _query = @"
                       select Admin_Name from dbo.Admin";

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

        [Route("api/Admin/saveFile")]
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

        [Route("api/admin/checkadminprofile")]
        [HttpGet]
        public HttpResponseMessage getAdminProfile(String email)
        {
            List<userAdmin> profile = new List<userAdmin>();
            List<userAdmin> theProfile = new List<userAdmin>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Admin_ID, Admin_Name, Admin_Surname, Admin_Contact, Admin_Email, Admin_Password, PhotoFileName from dbo.Admin", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userAdmin prof = new userAdmin();
                    prof.Admin_Name = reader["Admin_Name"].ToString();
                    prof.Admin_Surname = reader["Admin_Surname"].ToString();
                    prof.Admin_Contact = reader["Admin_Contact"].ToString();
                    prof.Admin_Email = reader["Admin_Email"].ToString();
                    prof.Admin_Password = reader["Admin_Password"].ToString();
                    prof.PhotoFileName = reader["PhotoFileName"].ToString();
                    profile.Add(prof);
                }
                foreach (userAdmin theProf in profile)
                {
                    if (theProf.Admin_Email == email)
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
                    return Request.CreateResponse(HttpStatusCode.OK, "No Admin Records Found");
                }
            }
        }

    }
}
