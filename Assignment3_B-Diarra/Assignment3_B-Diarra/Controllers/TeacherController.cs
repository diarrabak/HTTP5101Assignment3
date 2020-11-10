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
        /// <param name="id"></param>
        /// <example>Teacher/Show/T378 </example>
		/// <example>Teacher/Show/T381 </example>
        /// <returns>Teacher name, employee number, hire date and modules taught to view</returns>
        // 
        
        public ActionResult Show(string id)
        {
            string employeeNumber = id;
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.displayTeacher(employeeNumber);

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
        /// <returns>Teacher name, hire date and salary to view</returns>

        public ActionResult Search(string fname, string lname,DateTime? hireDate,decimal? salary)
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.Search(fname,lname,hireDate,salary);
            return View(selectedTeacher);
        }

    }
}