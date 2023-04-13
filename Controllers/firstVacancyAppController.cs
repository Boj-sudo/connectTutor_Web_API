using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace Web_API.Controllers
{
    public class firstVacancyAppController : ApiController
    {
        //fetching information from SQL Database using GET
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Applicant_ID, Firstname, Lastname, Contact_Number, Applicant_Email
                   from dbo.firstAppTable
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
        public string Post(firstVacancyApp _vacancy)
        {
            try
            {
                string _query = @"
                       insert into dbo.firstAppTable values
                       (
                            '" + _vacancy.Firstname + @"'
                            ,'" + _vacancy.Lastname + @"'
                            ,'" + _vacancy.Contact_Number + @"'
                            ,'" + _vacancy.Applicant_Email + @"'
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
        public string Put(firstVacancyApp _vacancy)
        {
            try
            {
                string _query = @"
                       update dbo.firstAppTable set
                       Firstname='" + _vacancy.Firstname + @"'
                       ,Lastname='" + _vacancy.Lastname + @"'
                       ,Contact_Number='" + _vacancy.Contact_Number + @"'
                       ,Applicant_Email='" + _vacancy.Applicant_Email + @"'
                       where Applicant_ID_ID=" + _vacancy.Applicant_ID + @"
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
                       delete from dbo.firstAppTable
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
    }
}
