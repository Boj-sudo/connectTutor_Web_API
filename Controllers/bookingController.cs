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
    public class bookingController : ApiController
    {
        //fetching information from SQL Database using GET
        public HttpResponseMessage Get()
        {
            string _query = @"
                   select Booking_ID, Student_Email, Module_Name, Tutor_Email, Start_Date, Start_Time, End_Time
                   from dbo.Booking
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
        public string Post(booking _booking)
        {
            try
            {
                string _query = @"
                       insert into dbo.Booking values
                       (
                            '" + _booking.Student_Email + @"'
                            ,'" + _booking.Module_Name + @"'
                            ,'" + _booking.Tutor_Email + @"'
                            ,'" + _booking.Start_Date + @"'
                            ,'" + _booking.Start_Time + @"'
                            ,'" + _booking.End_Time + @"'
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

                return "Added Booking Information Successfully.";
            }
            catch (Exception)
            {
                return "Failed To Add Booking Information";
            }
        }

        //Delete method
        public string Delete(int id)
        {
            try
            {
                string _query = @"
                       delete from dbo.Booking 
                       where Booking_ID=" + id + @"
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

                return "Cancelled Booking Information Successfully!!";
            }
            catch (Exception)
            {
                return "Failed To Cancel Booking Information";
            }
        }

        //custome method to return all student emails
        [Route("api/Student/GetAllStudentEmails")]
        [HttpGet]
        public HttpResponseMessage GetAllStudentEmails()
        {
            string _query = @"
                       select Student_Email from dbo.Student";

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

        //custome method to return all tutor names
        [Route("api/Tutor/GetAllTutorEmails")]
        [HttpGet]
        public HttpResponseMessage GetAllTutorEmails()
        {
            string _query = @"
                       select Tutor_Email from dbo.Tutor";

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

        //custome method to return all modules names
        [Route("api/Module/GetAllModulesByName")]
        [HttpGet]
        public HttpResponseMessage GetAllModulesName()
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

        [Route("api/booking/checkemail")]
        [HttpGet]
        public HttpResponseMessage getBookingByTutEmail(String email)
        {
            List<booking> bookings = new List<booking>();
            List<booking> theBookings = new List<booking>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Booking_ID, Student_Email,Module_Name, Tutor_Email,Start_Date,Start_Time,End_Time from dbo.Booking", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    booking book = new booking();
                    book.Student_Email = reader["Student_Email"].ToString();
                    book.Module_Name = reader["Module_Name"].ToString();
                    book.Tutor_Email = reader["Tutor_Email"].ToString();
                    book.Start_Date = reader["Start_Date"].ToString();
                    book.Start_Time = reader["Start_Time"].ToString();
                    book.End_Time = reader["End_Time"].ToString();
                    bookings.Add(book);
                }
                foreach (booking theBook in bookings)
                {
                    if (theBook.Tutor_Email == email)
                    {
                        isFound = true;
                        theBookings.Add(theBook);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theBookings);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Booking Records Found");
                }
            }
        }

        [Route("api/booking/countBooking")]
        [HttpGet]
        public HttpResponseMessage CountNumBooking(string module) ///Count number of bookings per module
        {
            List<booking> Bookings = new List<booking>();
            int counter = 0;
            bool hasBeenBooked = false;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Booking_ID,Student_Email,Module_Name,Tutor_Email,Start_Date,Start_Time,End_Time from dbo.Booking", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    booking book = new booking();
                    book.Student_Email = reader["Student_Email"].ToString();
                    book.Module_Name = reader["Module_Name"].ToString();
                    book.Tutor_Email = reader["Tutor_Email"].ToString();
                    book.Start_Date = reader["Start_Date"].ToString();
                    book.Start_Time = reader["Start_Time"].ToString();
                    book.End_Time = reader["End_Time"].ToString();
                    Bookings.Add(book);
                }
                foreach (booking book in Bookings)
                {
                    if (book.Module_Name == module)
                    {
                        hasBeenBooked = true;
                        counter++;
                    }
                }
                if (hasBeenBooked)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, counter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Module Records Found");
                }
            }
        }

        [Route("api/booking/countBookings")]
        [HttpGet]
        public HttpResponseMessage CountNumBookings() ///Count Total Number of Bookings
        {
            List<booking> _booking = new List<booking>();
            int counter = 0;


            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Student_Email, Module_Name,Tutor_Email, Start_Date, Start_Time, End_Time from dbo.Booking", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    booking _book = new booking();
                    _book.Student_Email = reader["Student_Email"].ToString();
                    _book.Module_Name = reader["Module_Name"].ToString();
                    _book.Tutor_Email = reader["Tutor_Email"].ToString();
                    _book.Start_Date = reader["Start_Date"].ToString();
                    _book.Start_Time = reader["Start_Time"].ToString();
                    _book.End_Time = reader["End_Time"].ToString();
                    _booking.Add(_book);
                }
                foreach (booking bk in _booking)
                {
                    counter++;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, counter);
        }


        [Route("api/booking/checkstudentemail")]
        [HttpGet]
        public HttpResponseMessage getBookingByStudentEmail(String email)
        {
            List<booking> bookings = new List<booking>();
            List<booking> theBookings = new List<booking>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Booking_ID,Student_Email,Module_Name,Tutor_Email,Start_Date,Start_Time,End_Time from dbo.Booking", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    booking book = new booking();
                    book.Student_Email = reader["Student_Email"].ToString();
                    book.Module_Name = reader["Module_Name"].ToString();
                    book.Tutor_Email = reader["Tutor_Email"].ToString();
                    book.Start_Date = reader["Start_Date"].ToString();
                    book.Start_Time = reader["Start_Time"].ToString();
                    book.End_Time = reader["End_Time"].ToString();
                    bookings.Add(book);
                }
                foreach (booking theBook in bookings)
                {
                    if (theBook.Student_Email == email)
                    {
                        isFound = true;
                        theBookings.Add(theBook);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theBookings);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Booking Records Found");
                }
            }
        }

        [Route("api/booking/checkbookedmodules")]
        [HttpGet]
        public HttpResponseMessage getBookedModules(String email)
        {
            List<booking> bookings = new List<booking>();
            List<booking> theBookings = new List<booking>();
            bool isFound = false;

            using (SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["EducationAppDB"].ConnectionString))
            {
                sql.Open();
                SqlCommand cmd = new SqlCommand("select Booking_ID,Student_Email,Module_Name,Tutor_Email,Start_Date,Start_Time,End_Time from dbo.Booking", sql);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    booking book = new booking();
                    book.Student_Email = reader["Student_Email"].ToString();
                    book.Module_Name = reader["Module_Name"].ToString();
                    book.Tutor_Email = reader["Tutor_Email"].ToString();
                    book.Start_Date = reader["Start_Date"].ToString();
                    book.Start_Time = reader["Start_Time"].ToString();
                    book.End_Time = reader["End_Time"].ToString();
                    bookings.Add(book);
                }
                foreach (booking theBook in bookings)
                {
                    if (theBook.Student_Email == email)
                    {
                        isFound = true;
                        theBookings.Add(theBook);
                    }
                }
                if (isFound)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, theBookings);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No Booking Records Found");
                }
            }
        }
    }
}
