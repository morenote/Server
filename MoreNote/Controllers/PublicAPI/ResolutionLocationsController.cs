using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Controllers.PublicAPI
{
    public class ResolutionLocationsController : Controller
    {
        private readonly DataContext _context;

        public ResolutionLocationsController(DataContext context)
        {
            _context = context;
        }

        // GET: ResolutionLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ResolutionLocation.ToListAsync().ConfigureAwait(false));
        }

        // GET: ResolutionLocations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionLocation = await _context.ResolutionLocation
                .FirstOrDefaultAsync(m => m.ResolutionLocationID == id).ConfigureAwait(false);
            if (resolutionLocation == null)
            {
                return NotFound();
            }

            return View(resolutionLocation);
        }

        // GET: ResolutionLocations/GenerateImage
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResolutionLocations/GenerateImage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("ResolutionLocationID,StrategyID,URL,Score,Weight,Speed")] ResolutionLocation resolutionLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resolutionLocation);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(resolutionLocation);
        }

        // GET: ResolutionLocations/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionLocation = await _context.ResolutionLocation.FindAsync(id).ConfigureAwait(false);
            if (resolutionLocation == null)
            {
                return NotFound();
            }
            return View(resolutionLocation);
        }

        // POST: ResolutionLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(long id, [Bind("ResolutionLocationID,StrategyID,URL,Score,Weight,Speed")] ResolutionLocation resolutionLocation)
        {
            if (id != resolutionLocation.ResolutionLocationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resolutionLocation);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResolutionLocationExists(resolutionLocation.ResolutionLocationID))
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
            return View(resolutionLocation);
        }

        // GET: ResolutionLocations/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionLocation = await _context.ResolutionLocation
                .FirstOrDefaultAsync(m => m.ResolutionLocationID == id).ConfigureAwait(false);
            if (resolutionLocation == null)
            {
                return NotFound();
            }

            return View(resolutionLocation);
        }

        // POST: ResolutionLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var resolutionLocation = await _context.ResolutionLocation.FindAsync(id).ConfigureAwait(false);
            _context.ResolutionLocation.Remove(resolutionLocation);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool ResolutionLocationExists(long id)
        {
            return _context.ResolutionLocation.Any(e => e.ResolutionLocationID == id);
        }
    }
}
