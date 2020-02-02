using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.HostDB
{
    public class HostServiceProvidersController : Controller
    {


        // GET: HostServiceProviders
        public async Task<IActionResult> Index()
        {
            using (var _context = new DataContext())
            {
                return View(await _context.HostServiceProvider.ToListAsync());
            }
        }

        // GET: HostServiceProviders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hostServiceProvider = await _context.HostServiceProvider
                    .FirstOrDefaultAsync(m => m.HostServiceProviderId == id);
                if (hostServiceProvider == null)
                {
                    return NotFound();
                }
                var secretReports = await _context.SecretReport.Where(m=>m.hostServiceProviderId==hostServiceProvider.HostServiceProviderId).ToArrayAsync();
                ViewBag.secretReports= secretReports;

                var company = await _context.ServiceProviderCompany.FirstOrDefaultAsync(m=>m.ServiceProviderCompanyId==hostServiceProvider.ServiceProviderCompanyId);
                ViewBag.company = company;
                return View(hostServiceProvider);
            }
        }

        // GET: HostServiceProviders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HostServiceProviders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HostServiceProviderId,HostName,ServiceProviderId,ServiceType,ISP,BeiAnGov,WebSite,OldWebSite,RegistrationPlace,FoundDate,IsBlock,RiskIndex,MentionByName,AnomalyDetection")] HostServiceProvider hostServiceProvider)
        {
            using (var _context = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(hostServiceProvider);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(hostServiceProvider);
            }
        }

        // GET: HostServiceProviders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hostServiceProvider = await _context.HostServiceProvider.FindAsync(id);
                if (hostServiceProvider == null)
                {
                    return NotFound();
                }
                return View(hostServiceProvider);
            }
        }

        // POST: HostServiceProviders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HostServiceProviderId,HostName,ServiceProviderId,ServiceType,ISP,BeiAnGov,WebSite,OldWebSite,RegistrationPlace,FoundDate,IsBlock,RiskIndex,MentionByName,AnomalyDetection")] HostServiceProvider hostServiceProvider)
        {
            using (var _context = new DataContext())
            {
                if (id != hostServiceProvider.HostServiceProviderId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(hostServiceProvider);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HostServiceProviderExists(hostServiceProvider.HostServiceProviderId))
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
                return View(hostServiceProvider);
            }
        }

        // GET: HostServiceProviders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hostServiceProvider = await _context.HostServiceProvider
                    .FirstOrDefaultAsync(m => m.HostServiceProviderId == id);
                if (hostServiceProvider == null)
                {
                    return NotFound();
                }

                return View(hostServiceProvider);
            }
        }

        // POST: HostServiceProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var _context = new DataContext())
            {
                var hostServiceProvider = await _context.HostServiceProvider.FindAsync(id);
                _context.HostServiceProvider.Remove(hostServiceProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool HostServiceProviderExists(long id)
        {
            using (var _context = new DataContext())
            {
                return _context.HostServiceProvider.Any(e => e.HostServiceProviderId == id);
            }
        }
    }
}
