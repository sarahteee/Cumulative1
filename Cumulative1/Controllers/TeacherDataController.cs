using Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;

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
        /// List of teacher objects
        /// </returns>
        /// <example>
        /// GET api/TeacherData/ListTeachers -> [{"TeacherId":"1", "TeacherFName":"Alexander", "TeacherLName":"Benett", "EmployeeNumber":"T378", "HireDate":"2016-08-05 00:00:00", "Salary":"55.30"
        /// GET api/TeacherData/ListTeachers -> [{"TeacherId":"2", "TeacherFName":"Caitlin", "TeacherLName":"Cummings", "EmployeeNumber":"T381", "HireDate":"2014-06-10 00:00:00", "Salary":"62.77"
        /// </example>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            //create connection
            MySqlConnection Conn = School.AccessDatabase();

            //open connection
            Conn.Open();

            //create command
            MySqlCommand cmd = Conn.CreateCommand();

            //command text SQL query
            cmd.CommandText = "select * from teachers";

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
                DateTime HireDate = (DateTime)ResultSet
                ["hiredate"];

                //get teacher salary
                double Salary = (double) ResultSet
                ["salary"];

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
    }
}
