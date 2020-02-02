using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.HostDB
{
    public class SecretReportsController : Controller
    {
     

        // GET: SecretReports
        public async Task<IActionResult> Index()
        {
            using (var _context = new DataContext())
            {
                return View(await _context.SecretReport.ToListAsync());
            }
        }

        // GET: SecretReports/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var secretReport = await _context.SecretReport
                .FirstOrDefaultAsync(m => m.SecretReportId == id);
            if (secretReport == null)
            {
                return NotFound();
            }

            return View(secretReport);
            }
        }

        // GET: SecretReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecretReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SecretReportId,hostServiceProviderId,serviceProviderCompanyId,ReporterId,IsRisk,ReportContent")] SecretReport secretReport)
        {
            using (var _context = new DataContext())
            {
                if (ModelState.IsValid)
            {
                _context.Add(secretReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(secretReport);
            }
        }

        // GET: SecretReports/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var secretReport = await _context.SecretReport.FindAsync(id);
            if (secretReport == null)
            {
                return NotFound();
            }
            return View(secretReport);
                }
        }

        // POST: SecretReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SecretReportId,hostServiceProviderId,serviceProviderCompanyId,ReporterId,IsRisk,ReportContent")] SecretReport secretReport)
        {
            using (var _context = new DataContext())
            {
                if (id != secretReport.SecretReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secretReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretReportExists(secretReport.SecretReportId))
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
            return View(secretReport);
            }
        }

        // GET: SecretReports/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var secretReport = await _context.SecretReport
                .FirstOrDefaultAsync(m => m.SecretReportId == id);
            if (secretReport == null)
            {
                return NotFound();
            }

            return View(secretReport);
            }
        }

        // POST: SecretReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var _context = new DataContext())
            {
                var secretReport = await _context.SecretReport.FindAsync(id);
            _context.SecretReport.Remove(secretReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
        }

        private bool SecretReportExists(long id)
        {
            using (var _context = new DataContext())
            {
                return _context.SecretReport.Any(e => e.SecretReportId == id);
            }
        }
    }
}
