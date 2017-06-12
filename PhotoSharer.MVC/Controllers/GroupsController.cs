using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoSharer.Business;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using PhotoSharer.Business.Services;
using PhotoSharer.MVC.Attributes;
using PhotoSharer.MVC.Helpers;
using PhotoSharer.MVC.NInjectIOC;
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

        private readonly UserService userService;

        private readonly PhotoStreamService photoStreamService;

        public GroupsController()
        {
            groupsService = IOC.Resolve<GroupsService>();
            userService = IOC.Resolve<UserService>();
            photoStreamService = IOC.Resolve<PhotoStreamService>();
        }

        [HttpGet]
        public ActionResult MyGroups()
        {
            var userId = IdentityHelper.UserId;
            var groups = groupsService.GetUserGroups(userId);

            if (groups == null)
                groups = new List<AppGroup>(0);

            var groupsItemList = Mapper.Map<IList<GroupListPageItemViewModel>>(groups);

            return View(groupsItemList);
        }

        [HttpGet]
        public ActionResult OwnGroups()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var groups = groupsService.GetCreatedByUser(userId);

            if (groups == null)
                groups = new List<AppGroup>(0);

            var groupsItemList = Mapper.Map<IList<GroupListPageItemViewModel>>(groups);

            return View(groupsItemList);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(Guid? id, string link)
        {
            if (!IdentityHelper.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsolutePath });

            if (!id.HasValue || string.IsNullOrEmpty(link))
                return HttpNotFound();

            var userId = IdentityHelper.UserId;

            var group = groupsService.GetByGroupInfo(id.Value, link);
            if (group == null)
                return HttpNotFound();

            var model = Mapper.Map<GroupViewModel>(group);

            model.IsMember = userService.IsInGroup(userId, group.Id);

            if (model.IsMember)
                model.IsCreator = group.CreatorId == userId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Transaction]
        public ActionResult JoinGroup(Guid groupId, string groupLink)
        {
            var userId = IdentityHelper.UserId;
            groupsService.AddUser(userId, groupId);
            return RedirectToRoute("GroupDetails", new { id = groupId, link = groupLink });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Transaction]
        public ActionResult LeaveGroup(Guid groupId, string groupLink)
        {
            var userId = IdentityHelper.UserId;

            groupsService.RemoveUser(userId, groupId);

            return RedirectToRoute("GroupDetails", new { id = groupId, link = groupLink });
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [Transaction]
        public ActionResult CreateGroup(CreateGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var group = groupsService.CreateGroup(model.Name, Guid.Parse(User.Identity.GetUserId()));

            return RedirectToRoute("GroupDetails", new { id = group.Id, link = group.Link });
        }

        [HttpGet]
        public ActionResult MyPhotoStreams(Guid id, string link)
        {
            var group = groupsService.GetByGroupInfo(id, link);

            if (group == null)
                return HttpNotFound();

            var photoStreams = photoStreamService.GetGroupStreamsByUserId(group.Id, IdentityHelper.UserId);
            var photoStreamsViewModelsList = Mapper.Map<IList<PhotoStreamViewModel>>(photoStreams);

            var model = Mapper.Map<PhotoStreamsListViewModel>(group);
            model.PhotoStreams = photoStreamsViewModelsList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Transaction]
        public ActionResult DeletePhotoStream(Guid? streamId, Guid? returnGroupId, string returnGroupLink)
        {
            if (streamId.HasValue)
            {
                if (photoStreamService.IsUsersPhotoStream(IdentityHelper.UserId, streamId.Value))
                {
                    photoStreamService.DeleteStream(streamId.Value);

                    if (returnGroupId.HasValue && !string.IsNullOrEmpty(returnGroupLink))
                        return RedirectToRoute("MyPhotoStreams", new { id = returnGroupId, link = returnGroupLink });
                }
            }
            return RedirectToRoute("MyGroups");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Transaction]
        public ActionResult AddPhotoStream(CreatePhotoStremViewModel model, Guid? returnGroupId, string returnGroupLink)
        {
            if (!ModelState.IsValid)
            {
                //TODO
                if (returnGroupId.HasValue && !string.IsNullOrEmpty(returnGroupLink))
                    return RedirectToRoute("MyPhotoStreams", new { id = returnGroupId, link = returnGroupLink });
                else
                    return RedirectToRoute("MyGroups");
            }

            if (!userService.IsInGroup(IdentityHelper.UserId, model.GroupId))
                return RedirectToRoute("MyGroups");

            if (photoStreamService.IsStreamInGroup(model.GroupId, model.Provider, model.Url))
                return RedirectToRoute("MyGroups");

            photoStreamService.CreatePhotoStream(IdentityHelper.UserId, model.GroupId, model.Provider, model.Url);

            if (returnGroupId.HasValue && !string.IsNullOrEmpty(returnGroupLink))
                return RedirectToRoute("MyPhotoStreams", new { id = returnGroupId, link = returnGroupLink });
            else
                return RedirectToRoute("MyGroups");
        }
    }
}
