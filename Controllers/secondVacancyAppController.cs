using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Web_API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Web_API.Controllers
{
    public class secondVacancyAppController : ApiController
    {
        //fetching information from SQL Database using GET
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Applicant_ID, Undergraduate_College, Undergraduate_Majors, Undergraduate_Degree_Type, 
                    Degree_Completion_Year, Teaching_Certificate
                   from dbo.secondAppTable
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

        //added information using POST method
        public string Post(secondVacancyApp vacancy)
        {
            try
            {
                string _query = @"
                       insert into dbo.secondAppTable values
                       (
                            '" + vacancy.Undergraduate_College + @"'
                            ,'" + vacancy.Undergraduate_Majors + @"'
                            ,'" + vacancy.Undergraduate_Degree_Type + @"'
                            ,'" + vacancy.Degree_Completion_Year + @"'
                            ,'" + vacancy.Teaching_Certificate + @"'
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
        public string Put(secondVacancyApp _vacancy)
        {
            try
            {
                string _query = @"
                       update dbo.secondAppTable set
                       ,Undergraduate_College='" + _vacancy.Undergraduate_College + @"'
                       ,Undergraduate_Majors='" + _vacancy.Undergraduate_Majors + @"'
                       ,Undergraduate_Degree_Type='" + _vacancy.Undergraduate_Degree_Type + @"'
                       ,Degree_Completion_Year='" + _vacancy.Degree_Completion_Year + @"'
                       ,Teaching_Certificate='" + _vacancy.Teaching_Certificate + @"'
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
                       delete from dbo.secondAppTable
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
