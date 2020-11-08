using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment3_B_Diarra.Models;

namespace Assignment3_B_Diarra.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This method put in a list all students returned by the Query
        /// <example>GET: Student/Studentlist </example>
        /// </summary>
        /// <returns>it returns the list of students with name, student number and enrollment date</returns>
        // 
        public ActionResult studentList()
        {
            StudentDataController list = new StudentDataController();
            List<Student> studentList = list.ListStudents().ToList();
            return View(studentList);
        }
    }
}