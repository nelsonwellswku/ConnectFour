﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Play()
        {
            return View();
        }
    }
}