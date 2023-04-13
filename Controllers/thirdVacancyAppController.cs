using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Web_API.Controllers
{
    public class thirdVacancyAppController : ApiController
    {
        //fetching information from SQL Database using GET
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Applicant_ID, CV, Matric_Certificate, ID_Copy, Other_Document_1, Other_Document_2, Other_Document_3
                   from dbo.thirdAppTable
                   ";

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

        //update information using POST method
        public string Post(thirdVacancyApp _vacancy)
        {
            try
            {
                string _query = @"
                       insert into dbo.thirdAppTable values
                       (
                            '" + _vacancy.CV + @"'
                            ,'" + _vacancy.Matric_Certificate + @"'
                            ,'" + _vacancy.ID_Copy + @"'
                            ,'" + _vacancy.Other_Document_1 + @"'
                            ,'" + _vacancy.Other_Document_2 + @"'
                            ,'" + _vacancy.Other_Document_3 + @"'
                       )";

                //Creating a Data Table to store information coming from database table
                DataTable _table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(_table);
                }

                return "Added Applicant Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Add Applicant Information.";
            }
        }

        //Put method
        public string Put(thirdVacancyApp _vacancy)
        {
            try
            {
                string _query = @"
                       update dbo.thirdAppTable set
                       CV='" + _vacancy.CV + @"'
                       ,Matric_Certificate='" + _vacancy.Matric_Certificate + @"'
                       ,ID_Copy='" + _vacancy.ID_Copy + @"'
                       ,Other_Document_1='" + _vacancy.Other_Document_1 + @"'
                       ,Other_Document_2='" + _vacancy.Other_Document_2 + @"'
                       ,Other_Document_3='" + _vacancy.Other_Document_3 + @"'
                       where Applicant_ID=" + _vacancy.Applicant_ID + @"
                       ";

                //Creating a Data Table to store information coming from database table
                DataTable _table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(_table);
                }

                return "Updated Applicant Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Update Applicant Information.";
            }
        }

        //Delete method
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.thirdAppTable
                       where Applicant_ID=" + id + @"
                       ";

                //Creating a Data Table to store information coming from database table
                DataTable _table = new DataTable();

                using (var sql_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
                using (var sql_command = new SqlCommand(_query, sql_connection))
                using (var data_adapter = new SqlDataAdapter(sql_command))
                {
                    sql_command.CommandType = CommandType.Text;
                    data_adapter.Fill(_table);
                }

                return "Deleted Applicant Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Delete Applicant Information.";
            }
        }

        [Route("api/VacancyApp/saveFile")]
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
                var physical_path = HttpContext.Current.Server.MapPath("~/Uploaded_Docs/" + file_name);

                posted_file.SaveAs(physical_path);

                return file_name;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
