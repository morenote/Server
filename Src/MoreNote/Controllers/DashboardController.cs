﻿/**
 * copy from https://github.com/ldqk/Masuit.MyBlogs
 * MIT License
 * Change:init
 * */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers
{
	public class DashboardController : Controller
	{
		[Authorize(Roles = "Admin,SuperAdmin")]
		[HttpGet]
		public ActionResult FileManager()
		{
			return View();
		}
	}
}