using lastTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using lastTest.Repository;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using lastTest.DataBase;

namespace lastTest.Controllers
{
    [Authorize(Policy = "StudentOnly")]
    public class StudentController : Controller
    {
        private readonly CourseRepository _repository;
        private readonly MyContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger, CourseRepository repository, MyContext context)
        {
            _logger = logger;
            _repository = repository;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var studentCourses = _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.UserId == userId)
                .ToList();

            return View(studentCourses);
        }

        public IActionResult RegisterCourses()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var allCourses = _context.Courses.ToList();
            var registeredCourseIds = _context.StudentCourses
                .Where(sc => sc.UserId == userId)
                .Select(sc => sc.CourseId)
                .ToList();

            var model = new RegisterCoursesViewModel
            {
                AllCourses = allCourses,
                RegisteredCourseIds = registeredCourseIds
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterCourses(int[] selectedCourseIds)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingCourses = _context.StudentCourses
                .Where(sc => sc.UserId == userId)
                .Select(sc => sc.CourseId)
                .ToList();

            foreach (var courseId in selectedCourseIds)
            {
                if (!existingCourses.Contains(courseId))
                {
                    var studentCourse = new StudentCourse
                    {
                        UserId = userId,
                        CourseId = courseId
                    };
                    _context.StudentCourses.Add(studentCourse);
                }
            }

            var coursesToRemove = existingCourses.Except(selectedCourseIds).ToList();
            foreach (var courseId in coursesToRemove)
            {
                var studentCourse = _context.StudentCourses
                    .FirstOrDefault(sc => sc.UserId == userId && sc.CourseId == courseId);
                if (studentCourse != null)
                {
                    _context.StudentCourses.Remove(studentCourse);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int userId, int courseId)
        {
            var userCourse = _context.StudentCourses
                .Include(sc => sc.Course)
                .FirstOrDefault(sc => sc.UserId == userId && sc.CourseId == courseId);

            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int userId, int courseId)
        {
            var userCourse = _context.StudentCourses
                .FirstOrDefault(sc => sc.UserId == userId && sc.CourseId == courseId);

            if (userCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(userCourse);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
