using lastTest.DataBase;
using lastTest.Models;
using lastTest.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace lastTest.Controllers
{
    [Authorize(Policy = "TeacherOnly")]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly MyContext _context;

        public TeacherController(ITeacherRepository teacherRepository, MyContext context)
        {
            _teacherRepository = teacherRepository;
            _context = context;
        }

        public IActionResult Index(int userID)
        {
            
            var courses = _teacherRepository.GetCoursesByTeacher(userID);

            return View(courses);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _teacherRepository.GetCourseById((int)id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create(int id)
        {
            
            Course c = new Course() { TeacherId=id};
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                var teacherId = course.TeacherId;

                if (teacherId == 0)
                {
                    return BadRequest("Invalid teacher ID");
                }

                course.TeacherId = teacherId;
                _teacherRepository.AddCourse(course);
                _teacherRepository.Save();

                return RedirectToAction("Index", "Teacher", new { userID = teacherId });
            }
            return View(course);
        }

        public IActionResult Edit(int id)
        {
            var course = _teacherRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CourseId,Name,Description,TeacherId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }
        
            if (course.CourseId!=null && course.Name!=null)
            {
                try
                {
                    _teacherRepository.UpdateCourse(course);
                    _teacherRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_teacherRepository.GetCourseById(course.CourseId) == null)
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction("Index", "Teacher", new { userID = course.TeacherId });
            }
            return View(course);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _teacherRepository.GetCourseById((int)id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int CourseId)
        {
            var course = _teacherRepository.GetCourseById(CourseId);
            if (course == null)
            {
                return NotFound();
            }

            var studentCourses = _context.StudentCourses.Where(sc => sc.CourseId == CourseId);
            _context.StudentCourses.RemoveRange(studentCourses);
            _context.SaveChanges();

            _teacherRepository.DeleteCourse(CourseId);
            _teacherRepository.Save();
            return RedirectToAction("Index", "Teacher", new { userID = course.TeacherId });
        }
    }
}
