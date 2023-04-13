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
    public class sessionController : ApiController
    {
        //fetching information from SQL Database using GET
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Session_ID, Module_Name, Tutor_Email, Student_Email, Session_Date, Start_Time, End_Time, Session_Status
                   from dbo.Session
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
        public string Post(session _session)
        {
            try
            {
                string _query = @"
                       insert into dbo.Session values
                       (
                            '" + _session.Module_Name + @"'
                            '" + _session.Tutor_Email + @"'
                            ,'" + _session.Student_Email + @"'
                            ,'" + _session.Session_Date + @"'
                            ,'" + _session.Start_Time + @"'
                            ,'" + _session.End_Time + @"'
                            ,'" + _session.Session_Status + @"'
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

                return "Created Session Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Create Session";
            }
        }

        //Delete method
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Session 
                       where Session_ID=" + id + @"
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

                return "Cancelled Session Successfully!!";
            }
            catch (Exception)
            {
                return "Failed To Cancel Session";
            }
        }

        [Route("api/session/countSessions")]
        [HttpGet]
        public HttpResponseMessage CountNumSessions() ///Count Total Number of Sessions
        {
            List<session> _session = new List<session>();
            int counter = 0;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Session_ID, Module_Name, Tutor_Email, Student_Email, Session_Date, Start_Time, End_Time, Session_Status from dbo.Session", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    session _sess = new session();
                    _sess.Module_Name = reader["Module_Name"].ToString();
                    _sess.Tutor_Email = reader["Tutor_Email"].ToString();
                    _sess.Student_Email = reader["Student_Email"].ToString();
                    _sess.Session_Date = reader["Session_Date"].ToString();
                    _sess.Start_Time = reader["Start_Time"].ToString();
                    _sess.End_Time = reader["End_Time"].ToString();
                    _sess.Session_Status = reader["Session_Status"].ToString();
                    _session.Add(_sess);
                }
                foreach (session bk in _session)
                {
                    counter++;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, counter);
        }
    }
}
