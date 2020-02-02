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
    public class ReportersController : Controller
    {
        

        // GET: Reporters
        public async Task<IActionResult> Index()
        {
            using (var _context = new DataContext())
            {
                return View(await _context.Reporter.ToListAsync());
            }
        }

        // GET: Reporters/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var reporter = await _context.Reporter
                .FirstOrDefaultAsync(m => m.ReporterId == id);
            if (reporter == null)
            {
                return NotFound();
            }

            return View(reporter);
            }
        }

        // GET: Reporters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reporters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReporterId,Name,WebSite,IsIdentify")] Reporter reporter)
        {
            using (var _context = new DataContext())
            {
                if (ModelState.IsValid)
            {
                _context.Add(reporter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reporter);
            }
        }

        // GET: Reporters/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var reporter = await _context.Reporter.FindAsync(id);
            if (reporter == null)
            {
                return NotFound();
            }
            return View(reporter);
            }
        }

        // POST: Reporters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ReporterId,Name,WebSite,IsIdentify")] Reporter reporter)
        {
            using (var _context = new DataContext())
            {
                if (id != reporter.ReporterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporterExists(reporter.ReporterId))
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
            return View(reporter);
            }
        }

        // GET: Reporters/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var _context = new DataContext())
            {
                if (id == null)
            {
                return NotFound();
            }

            var reporter = await _context.Reporter
                .FirstOrDefaultAsync(m => m.ReporterId == id);
            if (reporter == null)
            {
                return NotFound();
            }

            return View(reporter);
            }
        }

        // POST: Reporters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var _context = new DataContext())
            {
                var reporter = await _context.Reporter.FindAsync(id);
            _context.Reporter.Remove(reporter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
        }

        private bool ReporterExists(long id)
        {
            using (var _context = new DataContext())
            {
                return _context.Reporter.Any(e => e.ReporterId == id);
            }
        }
    }
}
