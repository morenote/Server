using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.HostDB
{
    public class ServiceProviderLegalPersonsController : Controller
    {


        // GET: ServiceProviderLegalPersons
        public async Task<IActionResult> Index()
        {
            using (var _context = new DataContext())
            {
                return View(await _context.ServiceProviderLegalPerson.ToListAsync());
            }
        }

        // GET: ServiceProviderLegalPersons/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderLegalPerson = await _context.ServiceProviderLegalPerson
                    .FirstOrDefaultAsync(m => m.PersonId == id);
                if (serviceProviderLegalPerson == null)
                {
                    return NotFound();
                }

                return View(serviceProviderLegalPerson);
            }
        }

        // GET: ServiceProviderLegalPersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceProviderLegalPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Name,About")] ServiceProviderLegalPerson serviceProviderLegalPerson)
        {
            using (var _context = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(serviceProviderLegalPerson);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(serviceProviderLegalPerson);
            }
        }

        // GET: ServiceProviderLegalPersons/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderLegalPerson = await _context.ServiceProviderLegalPerson.FindAsync(id);
                if (serviceProviderLegalPerson == null)
                {
                    return NotFound();
                }
                return View(serviceProviderLegalPerson);
            }
        }

        // POST: ServiceProviderLegalPersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PersonId,Name,About")] ServiceProviderLegalPerson serviceProviderLegalPerson)
        {
            using (var _context = new DataContext())
            {
                if (id != serviceProviderLegalPerson.PersonId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(serviceProviderLegalPerson);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ServiceProviderLegalPersonExists(serviceProviderLegalPerson.PersonId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(serviceProviderLegalPerson);
            }
        }

        // GET: ServiceProviderLegalPersons/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderLegalPerson = await _context.ServiceProviderLegalPerson
                    .FirstOrDefaultAsync(m => m.PersonId == id);
                if (serviceProviderLegalPerson == null)
                {
                    return NotFound();
                }

                return View(serviceProviderLegalPerson);
            }
        }

        // POST: ServiceProviderLegalPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var _context = new DataContext())
            {
                var serviceProviderLegalPerson = await _context.ServiceProviderLegalPerson.FindAsync(id);
                _context.ServiceProviderLegalPerson.Remove(serviceProviderLegalPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ServiceProviderLegalPersonExists(long id)
        {
            using (var _context = new DataContext())
            {
                return _context.ServiceProviderLegalPerson.Any(e => e.PersonId == id);
            }
        }
    }
}
