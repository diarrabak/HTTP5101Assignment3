using Assignment3_B_Diarra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment3_B_Diarra.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher/Index
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This View is used to enter the information to be searched in the database
        /// </summary>
        /// <returns></returns>
        // GET: Teacher/SearchIndex
        public ActionResult SearchTeacher()
        {
            return View();
        }
        /// <summary>
        /// This method puts in a list all teachers returned by the Query
        /// <example>GET: Teacher/List </example>
        /// </summary>
        /// <returns>it returns the list of teachers with name, employee number and hire date</returns>
        // 
        public ActionResult List()
        {
            TeacherDataController list = new TeacherDataController();
            List<Teacher> teacherList = list.ListTeachers().ToList();
            return View(teacherList);
        }


        /// <summary>
        /// This method displays a teacher selected by his employee number. It also shows the modules taught by the teacher.
        /// </summary>
        /// <param name="teacherEmployeenumber"></param>
        /// <example>This function is called a form element from Index </example>
        /// <returns>Teacher name, employee number, hire date and modules taught</returns>
        // 
        public ActionResult Show(string teacherEmployeenumber)
        {
            if (teacherEmployeenumber == "")
            {
                teacherEmployeenumber = "T378";  //Set to the first teacher
            }
            TeacherDataController list = new TeacherDataController();
            List<Teacher> teacherList = list.ListTeachers().ToList();
            Teacher selectedTeacher = new Teacher();  //Teacher matching the employee number
            List<int> positions = new List<int>();
            int i = 0;

            // This loop determines the number of occurence of the teacher to determine the number of module taught
            foreach(Teacher current in teacherList)
            {
                if (current.employeeNumber == teacherEmployeenumber)
                {
                    positions.Add(i); //number of appearance in the list
                }
                i++;
            }
            
            selectedTeacher = teacherList[positions[0]];

            //This loop adds all the modules taught together to be displayed by the teacher
            for (int j = 1; j < positions.Count; j++)
            {
				//We concatenate all the modules taught by that teacher in strings
                if (selectedTeacher.moduleCode!="")
                {
                    selectedTeacher.moduleCode += "--------------------" + teacherList[positions[j]].moduleCode;
                    selectedTeacher.moduleName += "--------" + teacherList[positions[j]].moduleName;
                    selectedTeacher.startDate += " ------" + teacherList[positions[j]].startDate;
                    selectedTeacher.finishDate += " --------" + teacherList[positions[j]].finishDate;
                }
            }

            return View(selectedTeacher);
        }


        /// <summary>
        /// This method permits to search for a teacher whose name, hire date and salary are provided.
        /// </summary>
        /// <example>This function is called by a form element from SearchTeacher</example>
        /// <param name="fname"> A string containing teacher name</param>
        /// <param name="lname"> A string containing teacher name</param>
        /// <param name="hireDate">A date variable containing teacher hire date</param>
        /// <param name="salary">A decimal variable containing teacher salary</param>
        /// <returns>Teacher name, hire date and salary</returns>

        public ActionResult Search(string fname, string lname,DateTime? hireDate,decimal? salary)
        {
            //Unknown parameter automatically lead to teacher not found
            if (fname=="" || lname=="" || hireDate ==null || salary ==null)
            {
                fname = "Unknown";
                lname = "Unknown";
                hireDate = null;
                salary = null;
            }
            TeacherDataController list = new TeacherDataController();
            List<Teacher> teacherList = list.ListTeachers().ToList();
            Teacher selectedTeacher = new Teacher();
            //This loop compares the parameters to the teacher list and return a teacher if all the parameters are met
            foreach (Teacher current in teacherList)
            {
                if (current.firstName == fname && current.surName == lname && current.hireDate == hireDate && current.salary == salary)
                {
                    selectedTeacher = current;
                    break;
                }
                else
                {
                    selectedTeacher.firstName = "Unknown";
                    selectedTeacher.surName = "Unknown";
                    //selectedTeacher.hireDate = (DateTime)hireDate;
                    //selectedTeacher.salary = (Decimal)salary;
                }
            }

            return View(selectedTeacher);
        }

    }
}