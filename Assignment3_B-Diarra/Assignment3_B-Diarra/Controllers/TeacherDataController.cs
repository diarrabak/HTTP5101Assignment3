using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3_B_Diarra.Models;
using MySql.Data.MySqlClient;

namespace Assignment3_B_Diarra.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The school database context class allow us to access our MySQL school Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table of our school database.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/teacherData/ListTeachers</example>
        /// <returns>
        /// A list of teachers with the subjects they teach as well
        /// </returns>
        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //This query will get teachers and modules taught
            cmd.CommandText = "SELECT teachers.*,classcode,classname, startdate, finishdate FROM teachers LEFT JOIN classes ON classes.teacherid = teachers.teacherid";

            //This variable will contain the ghathered results
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of teacher information and modules taught
            List<Teacher> teacherAndModules = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Teacher professor = new Teacher();   //A temporary variable to store current teacher
                professor.firstName = ResultSet["teacherfname"].ToString();
                professor.surName = ResultSet["teacherlname"].ToString();
                professor.employeeNumber = ResultSet["employeenumber"].ToString();
                professor.hireDate = (DateTime)ResultSet["hiredate"];  //Cast the result to date type
                professor.salary = (Decimal)ResultSet["salary"];       // Cast result to decimal type
                //Modules taught by the teacher
                professor.moduleCode= ResultSet["classcode"].ToString();
                professor.moduleName = ResultSet["classname"].ToString();
                professor.startDate = ResultSet["startdate"].ToString();
                professor.finishDate = ResultSet["finishdate"].ToString();
                //Add the teacher and his module to the List
                teacherAndModules.Add(professor);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers and their modules
            return teacherAndModules;
        }
    }
}


//The original code of this function is from the Web Application professor Christine Bitt of Humber College.