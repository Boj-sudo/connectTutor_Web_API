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
    public class moduleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Module_ID, Module_Name, Module_Description, Module_Image
                   from dbo.Module
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


        //add information using POST method
        public string Post(module _module)
        {
            try
            {
                string _query = @"
                       insert into dbo.Module values
                       (
                            '" + _module.Module_Name + @"'
                            ,'" + _module.Module_Description + @"'
                            ,'" + _module.Module_Image + @"'
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

                return "Added Module Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Add Module Information.";
            }
        }

        //Delete method
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Module 
                       where Module_ID=" + id + @"
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

                return "Deleted Module Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Delete Module Information";
            }
        }

        //custome method to return all the modules names
        [Route("api/Module/GetAllModules")]
        [HttpGet]
        public HttpResponseMessage GetAllModules()
        {
            string _query = @"
                       select Module_Name from dbo.Module";

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
    }
}
