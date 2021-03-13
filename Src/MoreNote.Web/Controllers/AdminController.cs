using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : BaseController
    {
        private APPStoreInfoService APPStoreInfoService { get; set; }
        private AccessService AccessService { get; set; }
        private DataContext dataContext;
   
        public AdminController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            
            , IHttpContextAccessor accessor
            ,APPStoreInfoService aPPStoreInfoService
            , AccessService accessService,
            DataContext dataContext) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.AccessService = accessService;
            this.dataContext=dataContext;


            this.APPStoreInfoService = aPPStoreInfoService;
        }


        public async Task<IActionResult> Index()
        {
            
                //  return View(await _context.AppInfo.ToListAsync());
                return View(APPStoreInfoService.GetAPPList());
            
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await dataContext.AppInfo
                    .FirstOrDefaultAsync(m => m.appid == id).ConfigureAwait(false);
                if (appInfo == null)
                {
                    return NotFound();
                }

                return View(appInfo);
            
        }

        // GET: Admin/GenerateImage
        public IActionResult Create()
        {
            if (1 == 1)
                return NotFound();
        }

        // POST: Admin/GenerateImage
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create(
            [Bind("appid,appautor,appdetail,appname,apppackage,appdownurl,applogourl,appversion,imglist,appsize")]
            AppInfo appInfo)
        {
          
                if (ModelState.IsValid)
                {
                    dataContext.Add(appInfo);
                    await dataContext.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction(nameof(Index));
                }

                return View(appInfo);
            
        }


        public async Task<IActionResult> Edit(long? id)
        {
           
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await dataContext.AppInfo.FindAsync(id).ConfigureAwait(false);
                if (appInfo == null)
                {
                    return NotFound();
                }

                return View(appInfo);
            
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id,
            [Bind("appid,appautor,appdetail,appname,apppackage,appdownurl,applogourl,appversion,imglist,appsize")]
            AppInfo appInfo)
        {
           
                if (appInfo == null || id != appInfo.appid)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        dataContext.Update(appInfo);
                        await dataContext.SaveChangesAsync().ConfigureAwait(false);
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

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
          
                if (id == null)
                {
                    return NotFound();
                }

                var appInfo = await dataContext.AppInfo
                    .FirstOrDefaultAsync(m => m.appid == id).ConfigureAwait(false);
                if (appInfo == null)
                {
                    return NotFound();
                }

                return View(appInfo);
            
        }

        // POST: Admin/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            return NotFound();
        }

        private bool AppInfoExists(long? id)
        {
           
                return dataContext.AppInfo.Any(e => e.appid == id);
            
        }
    }
}