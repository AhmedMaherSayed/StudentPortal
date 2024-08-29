using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models.Entities;
using StudentPortal.Web.Models.ViewModels;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            if (viewModel is not null)
            {
                var student = new Student
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    Phone = viewModel.Phone,
                    Subscribed = viewModel.Subscribed
                };

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var student = await _context.Students.FirstOrDefaultAsync(student => student.Id == id);
            var student = await _context.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await _context.Students.FindAsync(viewModel.Id);

            if(student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = await _context.Students.FindAsync(viewModel.Id);

            if( student is not null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
