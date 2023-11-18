using Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient; 
using System.Diagnostics;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        //This controller accesses the teachers table of the school database.
        /// <summary>
        /// Returns a list of teachers in the database
        /// </summary>
        /// <returns>
        /// List of teacher objects with teacher names containing the search key
        /// </returns>
        /// <param name="TeacherSearchKey">The teachers being searched</param>
        /// <example>
        /// GET api/TeacherData/ListTeachers -> [{"TeacherId":"1", "TeacherFName":"Alexander", "TeacherLName":"Benett", "EmployeeNumber":"T378", "HireDate":"2016-08-05 00:00:00", "Salary":"55.30"
        /// GET api/TeacherData/ListTeachers -> [{"TeacherId":"2", "TeacherFName":"Caitlin", "TeacherLName":"Cummings", "EmployeeNumber":"T381", "HireDate":"2014-06-10 00:00:00", "Salary":"62.77"
        /// </example>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{TeacherSearchKey}")]
        public List<Teacher> ListTeachers(string TeacherSearchKey)
        {
            Debug.WriteLine("trying to do an api search for " +  TeacherSearchKey);
            //create connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //create command
            MySqlCommand cmd = Conn.CreateCommand();

            //command text SQL query
            cmd.CommandText = "select * from teachers where teacherfname like @key or teacherlname like @key";

            //sanitize the teacher search key input
            cmd.Parameters.AddWithValue("@key", "%" + TeacherSearchKey + "%");

            //get a result set for our response
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //set up teacher list
            List<Teacher> Teachers = new List<Teacher>();

            //loop through query results
            while (ResultSet.Read())
            {
                //access column info

                //get teacher id
                int TeacherId = (int)ResultSet["teacherid"];

                //get teacher first name
                string TeacherFName = ResultSet
                ["teacherfname"].ToString();

                //get teacher last name
                string TeacherLName = ResultSet
                ["teacherlname"].ToString();

                //get teacher employee number
                string EmployeeNumber = ResultSet
                ["employeenumber"].ToString();

                //get teacher hire date
                DateTime HireDate = (DateTime) ResultSet
                ["hiredate"];
                 
                //get teacher salary
                string Salary = (ResultSet)
                ["salary"].ToString();

                //create and set info for new teacher object
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFName = TeacherFName;
                NewTeacher.TeacherLName = TeacherLName;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                //add teacher to list
                Teachers.Add(NewTeacher);
            }

            //close connection between server and database
            Conn.Close();

            //return final list of teachers
            return Teachers;
        }

        /// <summary>
        /// Find a teacher by teacherid input
        /// </summary>
        /// <param name="TeacherId">The teacherid primary key in the database</param>
        /// <returns>
        /// Teacher object of the inputted teacherid
        /// </returns>
        /// <example>
        /// GET api/TeacherData/FindTeacher  -> [{"TeacherId":"3", "TeacherFName":"Linda", "TeacherLName":"Chan", "EmployeeNumber":"T382", "HireDate":"2015-08-22 00:00:00", "Salary":"60.22"
        /// GET api/TeacherData/FindTeacher  -> [{"TeacherId":"4", "TeacherFName":"Lauren", "TeacherLName":"Smith", "EmployeeNumber":"T385", "HireDate":"2014-06-22 00:00:00", "Salary":"74.20"
        /// </example>

        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{TeacherId}")]

        public Teacher FindTeacher(int TeacherId)
        {
            //create connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //create command
            MySqlCommand Command = Conn.CreateCommand();

            //set the command text
            Command.CommandText = "select * from teachers where teacherid = " + TeacherId;

            //get result set
            MySqlDataReader ResultSet = Command.ExecuteReader();
            
            Teacher SelectedTeacher = new Teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.TeacherId = Convert.ToInt32
                (ResultSet["teacherid"]);

                SelectedTeacher.TeacherFName = ResultSet
                ["teacherfname"].ToString();

                SelectedTeacher.TeacherLName = ResultSet
                ["teacherlname"].ToString();

                SelectedTeacher.EmployeeNumber = ResultSet
                ["employeenumber"].ToString();

                SelectedTeacher.HireDate = (DateTime)ResultSet
                ["hiredate"];

                SelectedTeacher.Salary = (ResultSet)
                ["salary"].ToString();

            }

            //close connection
            Conn.Close();

            return SelectedTeacher;
        }
    }
}
