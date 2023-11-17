using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative1.Models;
using System.Diagnostics;

namespace Cumulative1.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher/List?TeacherSearchKey={value}
        // Go to /Views/Teacher/List.cshtml
        // Browser opens a list teachers page
        public ActionResult List(string TeacherSearchKey)
        {
            //Check if the search key works
            Debug.WriteLine("I want to search for teachers with the key " + TeacherSearchKey);

            //pass teacher information to view
            //create instance of teacher data controller
            TeacherDataController Controller = new TeacherDataController();

            List<Teacher> Teachers = Controller.ListTeachers(TeacherSearchKey);

            //pass teacher information to /views/teacher/list
            return View(Teachers);
        }

        //GET : /Teacher/Show/{id}
        //Route to /Views/Teachers/Show.cshtml
        public ActionResult Show(int id) 
        {
            TeacherDataController Controller = new TeacherDataController();

            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

    }
}