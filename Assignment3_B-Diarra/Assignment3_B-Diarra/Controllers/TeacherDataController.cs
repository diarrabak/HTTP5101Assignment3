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
            cmd.CommandText = "SELECT * FROM  teachers";

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
                //Add the teacher and his module to the List
                teacherAndModules.Add(professor);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers and their modules
            return teacherAndModules;
        }


        /// <summary>
        /// This method displays a teacher selected by his employee number. It also shows the modules taught by the teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <example>api/Teacher/displayTeacher/T381 </example>
        /// <example>api/Teacher/displayTeacher/T378 </example>
        /// <returns>Teacher with the name, employee number, hire date and modules taught</returns>
        // 
       [Route("api/TeacherData/displayTeacher/{id}")]

        [HttpGet]
        public Teacher displayTeacher(string id)
        {
            string employeeNumber = id;
            Teacher selectedTeacher = new Teacher();
            if (employeeNumber == "")
            {
                employeeNumber = "T378";  //Set to the first teacher
            }

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //This query will get teachers and modules taught
            cmd.CommandText = "SELECT teachers.*,classcode,classname, startdate, finishdate FROM teachers LEFT JOIN classes ON classes.teacherid = teachers.teacherid WHERE employeenumber=\""+ employeeNumber + "\"";

            //This variable will contain the ghathered results
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of teacher information and modules taught
            List<Teacher> teacherAndModules = new List<Teacher> { };

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
                professor.moduleCode = ResultSet["classcode"].ToString();
                professor.moduleName = ResultSet["classname"].ToString();
                professor.startDate = ResultSet["startdate"].ToString();
                professor.finishDate = ResultSet["finishdate"].ToString();
                //Add the teacher and his module to the List
                teacherAndModules.Add(professor);
            }
            if (teacherAndModules.Count > 0)  selectedTeacher = teacherAndModules[0];
            
            if (teacherAndModules.Count > 1)
            {
                //This loop adds all the modules taught by the teacher together to be displayed
                for (int j = 1; j < teacherAndModules.Count; j++)
                {
                    //We concatenate all the modules taught by that teacher in strings
                    if (selectedTeacher.moduleCode != "")
                    {
                        selectedTeacher.moduleCode += "---------------------------" + teacherAndModules[j].moduleCode;
                        selectedTeacher.moduleName += "------------" + teacherAndModules[j].moduleName;
                        selectedTeacher.startDate += " ---------------" + teacherAndModules[j].startDate;
                        selectedTeacher.finishDate += " -----------------" + teacherAndModules[j].finishDate;
                    }
                }
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers and their modules
            return selectedTeacher;
        }


        /// <summary>
        /// This method permits to search for a teacher whose name, hire date and salary are provided.
        /// </summary>
        /// <example>Get  api/TeacherData/Search/Caitlin/Cummings/2014-06-10 00:00:00/62.77</example>
        /// <param name="fname"> A string containing teacher name</param>
        /// <param name="lname"> A string containing teacher name</param>
        /// <param name="hireDate">A date variable containing teacher hire date</param>
        /// <param name="salary">A decimal variable containing teacher salary</param>
        /// <returns>Teacher name, hire date and salary</returns>


        [Route("api/TeacherData/Search/{fname}/{lname}/{hireDate}/{salary}")]
        [HttpGet]
        public Teacher Search(string fname, string lname, DateTime? hireDate, decimal? salary)
        {
            //Unknown parameter automatically lead to teacher not found
            if (fname == "" || lname == "" || hireDate == null || salary == null)
            {
                fname = "Unknown";
                lname = "Unknown";
                hireDate = null;
                salary = null;
            }
            //Complete list of teachers from the database is used 
            TeacherDataController list = new TeacherDataController();
            List<Teacher> teacherList = list.ListTeachers().ToList();
            Teacher selectedTeacher = new Teacher();

            //This loop compares the parameters to the teacher list and return a teacher if all the parameters are met
            foreach (Teacher current in teacherList)
            {
                //Here all the properties must be met
                if (current.firstName == fname && current.surName == lname && current.hireDate == hireDate && current.salary == salary)
                {
                    selectedTeacher = current;
                    break;  //Stop as soon as the teacher found
                }
                else
                {
                    selectedTeacher.firstName = "Unknown";
                    selectedTeacher.surName = "Unknown";
                }
            }

            return selectedTeacher;
        }
    }
    
}


//NB: The original code of this function is from the Web Application professor Christine Bitt of Humber College.