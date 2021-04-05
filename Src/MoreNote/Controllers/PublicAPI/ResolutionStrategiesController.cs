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
    public class ResolutionStrategiesController : Controller
    {
        private readonly DataContext _context;

        public ResolutionStrategiesController(DataContext context)
        {
            _context = context;
        }

        // GET: ResolutionStrategies
        public async Task<IActionResult> Index()
        {
            return View(await _context.ResolutionStrategy.ToListAsync().ConfigureAwait(false));
        }

        // GET: ResolutionStrategies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionStrategy = await _context.ResolutionStrategy
                .FirstOrDefaultAsync(m => m.StrategyID == id).ConfigureAwait(false);
            if (resolutionStrategy == null)
            {
                return NotFound();
            }

            return View(resolutionStrategy);
        }

        // GET: ResolutionStrategies/GenerateImage
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResolutionStrategies/GenerateImage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("StrategyID,StrategyKey,StrategyName,CheckTime")] ResolutionStrategy resolutionStrategy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resolutionStrategy);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(resolutionStrategy);
        }

        // GET: ResolutionStrategies/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionStrategy = await _context.ResolutionStrategy.FindAsync(id).ConfigureAwait(false);
            if (resolutionStrategy == null)
            {
                return NotFound();
            }
            return View(resolutionStrategy);
        }

        // POST: ResolutionStrategies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(long? id, [Bind("StrategyID,StrategyKey,StrategyName,CheckTime")] ResolutionStrategy resolutionStrategy)
        {
            if (id != resolutionStrategy.StrategyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resolutionStrategy);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResolutionStrategyExists(resolutionStrategy.StrategyID))
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
            return View(resolutionStrategy);
        }

        // GET: ResolutionStrategies/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionStrategy = await _context.ResolutionStrategy
                .FirstOrDefaultAsync(m => m.StrategyID == id).ConfigureAwait(false);
            if (resolutionStrategy == null)
            {
                return NotFound();
            }

            return View(resolutionStrategy);
        }

        // POST: ResolutionStrategies/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var resolutionStrategy = await _context.ResolutionStrategy.FindAsync(id).ConfigureAwait(false);
            _context.ResolutionStrategy.Remove(resolutionStrategy);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool ResolutionStrategyExists(long? id)
        {
            return _context.ResolutionStrategy.Any(e => e.StrategyID == id);
        }
    }
}
