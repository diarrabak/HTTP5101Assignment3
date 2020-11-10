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
    public class StudentDataController : ApiController
    {
        // The school database context class allow us to access our MySQL school Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the students table of our school database.
        /// <summary>
        /// Returns a list of students in the system
        /// </summary>
        /// <example>GET api/studentData/ListStudents</example>
        /// <returns>
        /// A list of students
        /// </returns>
        [HttpGet]
        public IEnumerable<Student> ListStudents()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //This query will get student table
            cmd.CommandText = "SELECT* FROM students";

            //This variable will contain the ghathered results
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of student information 
            List<Student> students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Student student = new Student();   //A temporary variable to store current student
                student.firstName = ResultSet["studentfname"].ToString();
                student.surName = ResultSet["studentlname"].ToString();
                student.studentNumber = ResultSet["studentnumber"].ToString();
                student.enrolDate = (DateTime)ResultSet["enroldate"];  //Cast the result to date type
                //Add the student to the List
                students.Add(student);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of students
            return students;
        }
    }
}


//NB: The original code of this function is from the Web Application professor Christine Bittle of Humber College.