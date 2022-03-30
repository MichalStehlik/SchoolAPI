using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.ViewModels;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassroomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Classrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            return await _context.Classrooms.ToListAsync();
        }

        // GET: api/Classrooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound();
            }

            return Ok(classroom);
        }

        // PUT: api/Classrooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassroom(int id, Classroom classroom)
        {
            if (id != classroom.ClassroomId)
            {
                return BadRequest();
            }

            _context.Entry(classroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(classroom);
        }

        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom([FromBody] Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassroom", new { id = classroom.ClassroomId }, classroom);
        }

        // DELETE: api/Classrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();

            return Ok(classroom);
        }

        private bool ClassroomExists(int id)
        {
            return _context.Classrooms.Any(e => e.ClassroomId == id);
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<Classroom>> GetClassroomStudents(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound("class does not exists");
            }

            _context.Entry(classroom).Collection(c => c.Students).Load();

            return Ok(classroom.Students);
        }

        /*[HttpPost("{id}/students/{studentId}")]*/
        [HttpPost("{id}/students")]
        public async Task<ActionResult> AddStudentToClass(int id, [FromBody] IdVM student)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound("class does not exists");
            }
            var st = await _context.Students.FindAsync(student.Id);

            if (st == null)
            {
                return NotFound("student does not exists");
            }

            var stinclass = 
                await _context.Students.Where(s => s.StudentId == student.Id && s.ClassroomId == id).SingleOrDefaultAsync();
            if (stinclass == null)
            {
                st.ClassroomId = id;
                await _context.SaveChangesAsync();
                return Ok(st);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}/students/{studentId}")]
        public async Task<ActionResult> RemoveStudentFromClass(int id, int studentId)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound("class does not exists");
            }
            var st = await _context.Students.FindAsync(studentId);

            if (st == null)
            {
                return NotFound("student does not exists");
            }

            var stinclass =
                await _context.Students.Where(s => s.StudentId == studentId && s.ClassroomId == id).SingleOrDefaultAsync();
            if (stinclass == null)
            {
                return BadRequest();
            }
            else
            {
                st.ClassroomId = null;
                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete("{id}/students")]
        public async Task<ActionResult> ClearStudentsFromClass(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound("class does not exists");
            }

            _context.Entry(classroom).Collection(c => c.Students).Load();
            classroom.Students.Clear();
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
