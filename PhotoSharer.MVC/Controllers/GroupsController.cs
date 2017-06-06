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
        private readonly GroupsService groupsService;

        public GroupsController(GroupsService groupsService)
        {
            this.groupsService = groupsService;
        }


        [HttpGet]
        public ActionResult My()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var groups = groupsService.GetUserGroups(userId);

            if (groups == null)
            {
                groups = new List<AppGroup>(0);
            }

            var groupsItemList = groups.Select(group => new GroupListPageItemViewModel
            {
                Name = group.Name,
                Link = group.Link
            }).ToList();

            return View(groupsItemList);
        }


        [HttpGet]
        public ActionResult Owned()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var groups = groupsService.GetCreatedByUser(userId);

            if (groups == null)
            {
                groups = new List<AppGroup>(0);
            }

            var groupsItemList = groups.Select(group => new GroupListPageItemViewModel
            {
                Name = group.Name,
                Link = group.Link,
            }).ToList();

            return View(groupsItemList);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Group(string link)
        {
            var group = groupsService.GetByLink(link);
            if (group == null)
            {
                return HttpNotFound();
            }

            var model = new GroupViewModel
            {
                Name = group.Name,
                Link = group.Link
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinGroup(string link)
        {
            var result = groupsService.AddUser(Guid.Parse(User.Identity.GetUserId()), link);
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
                return RedirectToRoute("Group", new { link = group.Link });
            }

            return RedirectToAction("Index", "Groups");
        }
    }
}
