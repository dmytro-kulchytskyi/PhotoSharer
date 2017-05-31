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
    [Authorize]
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



        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var groups = groupsService.GetUserGroups(userId);

            if (groups == null)
            {
                groups = new List<AppGroup>(0);
            }
            //((System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity).AddClaim(new System.Security.Claims.Claim())
            var groupsItemList = groups.Select(group => new GroupListPageItemViewModel()
            {
                Name = group.Name,
                InviteCode = group.InviteCode,
                Url = group.Url
            }).ToList();

            return View(groupsItemList);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Group(string url)
        {
            return Content(url);
        }


        public ActionResult JoinGroup(string groupUrl)
        {
            var result = groupsService.AddUserToGroup(Guid.Parse(User.Identity.GetUserId()), groupUrl);
            return Content(result.ToString());
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateGroup(CreateGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var group = groupsService.CreateGroup(model.Name, Guid.Parse(User.Identity.GetUserId()));
            if (group != null)
            {
                return RedirectToAction("Group", "Groups", new { url = group });
            }

            return RedirectToAction("Index", "Groups");
        }
    }
}
