using ITB2203Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _context;

        public EventsController (DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Event>> GetEvents(string? name = null)
        {
            var query = _context.Events!.AsQueryable();

            if (name != null)
                query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<TextReader> GetEvent(int id)
        {
            var eve = _context.Events!.Find(id);

            if (eve == null)
            {
                return NotFound();
            }

            return Ok(eve);
        }

        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, Event eve)
        {
            var dbEvent = _context.Events!.AsNoTracking().FirstOrDefault(x => x.Id == eve.Id);
            if (id != eve.Id || dbEvent == null)
            {
                return NotFound();
            }

            _context.Update(eve);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Test> PostEvent(Event eve)
        {
            var dbExercise = _context.Events!.Find(eve.Id);
            if (dbExercise == null)
            {
                _context.Add(eve);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetEvent), new { Id = eve.Id }, eve);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var eve = _context.Events!.Find(id);
            if (eve == null)
            {
                return NotFound();
            }

            _context.Remove(eve);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
