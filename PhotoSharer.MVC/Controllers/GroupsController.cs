using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using PhotoSharer.Business.Services;
using PhotoSharer.MVC.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhotoSharer.MVC.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupRepository groupRepository;
        private readonly GroupsService groupsService;

        public GroupsController(
            IGroupRepository groupRepository,
            GroupsService groupsService)
        {
            this.groupsService = groupsService;
            this.groupRepository = groupRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var groups = groupsService.GetUserGroups(userId);

                if (groups == null)
                {
                    groups = new List<AppGroup>(0);
                }

                var groupsItemList = groups.Select(group => new GroupListPageItemViewModel()
                {
                    Name = group.Name,
                    InviteCode = group.InviteCode,
                    Url = group.Url
                });

                return View(groupsItemList);
        }

        [HttpGet]
        public ActionResult Group(string url)
        {
            //TODO
            return Content(url);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateGroup(CreateGroupViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var url = groupsService.CreateGroup(model.Name, Guid.Parse(User.Identity.GetUserId()));
            if (url != null)
            {
                return RedirectToRoute("Group", new { url = url });
            }

            return RedirectToAction("Index", "Groups");
        }

    }
}