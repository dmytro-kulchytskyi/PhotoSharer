using PhotoSharer.Models;
using PhotoSharer.Models.Repository;
using PhotoSharer.Models.Repository.Interface;
using PhotoSharer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharer.Controllers
{
    public class GroupsController : Controller
    {
        public GroupsController(IGroupRepository groupRepository)
        {
          GroupRepository = groupRepository;
        }

        private readonly IGroupRepository GroupRepository;



        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(CreateGroup model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppGroup newgroup = new AppGroup()
            {
                Name = model.Name,
                InviteCode = "123"
            };

            GroupRepository.Save(newgroup);
            return RedirectToAction("Index", "Home");
        }

    }
}