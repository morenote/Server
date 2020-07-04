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
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        //private readonly DataContext _context;



     
        public async Task<IActionResult> Index()
        {
            using (DataContext _context = new DataContext())
            {
              //  return View(await _context.AppInfo.ToListAsync());
                return View(APPStoreInfoService.GetAPPList());
            }
           
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (DataContext _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await _context.AppInfo
                    .FirstOrDefaultAsync(m => m.appid == id);
                if (appInfo == null)
                {
                    return NotFound();
                }

                return View(appInfo);
            }
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            if(1==1)
            return NotFound();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       // [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("appid,appautor,appdetail,appname,apppackage,appdownurl,applogourl,appversion,imglist,appsize")] AppInfo appInfo)
        {
           
            using (DataContext _context = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(appInfo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(appInfo);
            }
        }

        
        public async Task<IActionResult> Edit(long? id)
        {
           
            using (DataContext _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await _context.AppInfo.FindAsync(id);
                if (appInfo == null)
                {
                    return NotFound();
                }
                return View(appInfo);
            }
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        [ValidateAntiForgeryToken]
    
        public async Task<IActionResult> Edit(long id, [Bind("appid,appautor,appdetail,appname,apppackage,appdownurl,applogourl,appversion,imglist,appsize")] AppInfo appInfo)
        {
          
            using (DataContext _context = new DataContext())
            {
                if (id != appInfo.appid)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(appInfo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AppInfoExists(appInfo.appid))
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
                return View(appInfo);
            }
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            
            using (DataContext _context = new DataContext())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await _context.AppInfo
                    .FirstOrDefaultAsync(m => m.appid == id);
                if (appInfo == null)
                {
                    return NotFound();
                }

                return View(appInfo);
            }
        }

        // POST: Admin/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            return NotFound();
            using (DataContext _context = new DataContext())
            {
                var appInfo = await _context.AppInfo.FindAsync(id);
                _context.AppInfo.Remove(appInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool AppInfoExists(long id)
        {
            using (DataContext _context = new DataContext())
            {
                return _context.AppInfo.Any(e => e.appid == id);
            }
        }
    }
}
