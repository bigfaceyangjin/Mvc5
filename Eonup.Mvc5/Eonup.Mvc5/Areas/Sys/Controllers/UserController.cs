﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Areas.Sys.Controllers
{
    public class UserController : Controller
    {
        // GET: Sys/User
        public ActionResult Index()
        {
            return View();
        }
    }
}