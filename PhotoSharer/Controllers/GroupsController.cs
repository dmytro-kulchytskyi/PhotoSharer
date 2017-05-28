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
using PhotoSharer.Identity;

namespace PhotoSharer.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupRepository groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            int item = 10;
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var usersGroup = groupRepository.GetUserGroups(Guid.Parse(User.Identity.GetUserId()));
            ViewBag.pages = usersGroup.Count/ item;
            if(usersGroup.Count% item != 0) ViewBag.pages++;
             var usersGroups=usersGroup.OrderBy(n => n.Name).Skip((page - 1) * item).Take(item);
            return View(usersGroups);
        }

        [HttpGet]
        public ActionResult View(Guid id)
        {
            if (!User.Identity.IsAuthenticated)return RedirectToAction("Login", "Account");
            var group = groupRepository.GetGroupById(id);
            return View(group);
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