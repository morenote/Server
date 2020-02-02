using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.HostDB
{
    public class ServiceProviderCompaniesController : Controller
    {


        // GET: ServiceProviderCompanies
        public async Task<IActionResult> Index()
        {
            using (var _context = new DataContext())
            {
                return View(await _context.ServiceProviderCompany.ToListAsync());
            }
        }

        // GET: ServiceProviderCompanies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderCompany = await _context.ServiceProviderCompany
                    .FirstOrDefaultAsync(m => m.ServiceProviderCompanyId == id);
                if (serviceProviderCompany == null)
                {
                    return NotFound();
                }

                return View(serviceProviderCompany);
            }
        }

        // GET: ServiceProviderCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceProviderCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceProviderCompanyId,SPName,LegalPersonId,RegionDate,WebSite,OldWebSite,RegistrationPlace,FoundDate,IsBlock,RiskIndex,MentionByName,AnomalyDetection")] ServiceProviderCompany serviceProviderCompany)
        {
            using (var _context = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(serviceProviderCompany);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(serviceProviderCompany);
            }
        }

        // GET: ServiceProviderCompanies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderCompany = await _context.ServiceProviderCompany.FindAsync(id);
                if (serviceProviderCompany == null)
                {
                    return NotFound();
                }
                return View(serviceProviderCompany);
            }
        }

        // POST: ServiceProviderCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ServiceProviderCompanyId,SPName,LegalPersonId,RegionDate,WebSite,OldWebSite,RegistrationPlace,FoundDate,IsBlock,RiskIndex,MentionByName,AnomalyDetection")] ServiceProviderCompany serviceProviderCompany)
        {
            using (var _context = new DataContext())
            {
                if (id != serviceProviderCompany.ServiceProviderCompanyId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(serviceProviderCompany);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ServiceProviderCompanyExists(serviceProviderCompany.ServiceProviderCompanyId))
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
                return View(serviceProviderCompany);
            }
        }

        // GET: ServiceProviderCompanies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var serviceProviderCompany = await _context.ServiceProviderCompany
                    .FirstOrDefaultAsync(m => m.ServiceProviderCompanyId == id);
                if (serviceProviderCompany == null)
                {
                    return NotFound();
                }

                return View(serviceProviderCompany);
            }
        }

        // POST: ServiceProviderCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var _context = new DataContext())
            {
                var serviceProviderCompany = await _context.ServiceProviderCompany.FindAsync(id);
                _context.ServiceProviderCompany.Remove(serviceProviderCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ServiceProviderCompanyExists(long id)
        {
            using (var _context = new DataContext())
            {
                return _context.ServiceProviderCompany.Any(e => e.ServiceProviderCompanyId == id);
            }
        }
    }
}
