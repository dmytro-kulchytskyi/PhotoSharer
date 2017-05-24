using Microsoft.AspNet.Identity;
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
        private readonly IGroupRepository groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

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

            AppGroup group = new AppGroup()
            {
                Name = model.Name,
                InviteCode = "12310",
                Creator = new AppUser() { Id = Guid.Parse(User.Identity.GetUserId()) }
            };

            groupRepository.Save(group);
            return RedirectToAction("Index", "Home");
        }

    }
}