﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharer.Controllers
{
    public class GroupsController : Controller
    {
        // GET: Groups
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroup()
        {
            return View();
        }

    }
}