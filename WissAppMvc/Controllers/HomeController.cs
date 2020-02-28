﻿using AppCore.Services;
using AppCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WissAppEF.Contexts;
using WissAppEntities.Entities;
using WissAppMvc.Models;
using WissAppMvc.Models.ViewModels;
using WissAppMvc.Utils;

namespace WissAppMvc.Controllers
{
    public class HomeController : Controller
    {
        DbContext db = new WissAppContext();
        ServiceBase<Users> userService;
        ServiceBase<UsersMessages> userMessagesService;

        public HomeController()
        {
            userService = new Service<Users>(db);
            userMessagesService = new Service<UsersMessages>(db);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userMessages = userMessagesService.GetEntities(e => e.Senders.UserName == User.Identity.Name || e.Receivers.UserName == User.Identity.Name);
            var users = userMessages.Select(e => e.Receivers.UserName == User.Identity.Name ? 
            new UsersModel
            {
                UserId = e.SenderId,
                UserName = e.Senders.UserName
            } : new UsersModel
            {
                UserId = e.ReceiverId ?? 0,
                UserName = e.Receivers.UserName,
            }).ToList();
            //users = users.Distinct(new UsersModelComparer()).ToList();
            users = users.GroupBy(e => new
            {
                e.UserId,
                e.UserName
            }).Select(e => new UsersModel
            {
                UserId = e.Key.UserId,
                UserName = e.Key.UserName,
                MessageCount = e.Count()
            }).ToList();
            var model = new HomeIndexViewModel()
            {
                Users = users,
                Messages = new List<MessagesModel>()
            };
            return View(model);
        }

        [Authorize]
        public ActionResult Messages(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var userMessages = userMessagesService.GetEntities(e => (e.Senders.UserName == User.Identity.Name && e.ReceiverId == id) || (e.Receivers.UserName == User.Identity.Name && e.SenderId == id)).Select(e => new MessagesModel
                {
                    Message = e.Messages.Message,
                    Date = e.Messages.Date.ToShortDateString() + " " + e.Messages.Date.ToLongTimeString(),
                    User = e.SenderId == id ? e.Senders.UserName : e.Receivers.UserName,
                    Sent = e.SenderId == id
                }).ToList();
                var model = new HomeIndexViewModel()
                {
                    Messages = userMessages,
                    Users = new List<UsersModel>()
                };
                return PartialView("_Messages", model);
            }
            return new EmptyResult();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersLoginModel usersModel)
        {
            if (ModelState.IsValid)
            {
                if (userService.EntityExists(e => e.UserName == usersModel.UserName && e.Password == usersModel.Password && !e.IsDeleted && e.IsActive))
                {
                    FormsAuthentication.SetAuthCookie(usersModel.UserName, usersModel.RememberMe);
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "User Name or Password is incorrect!";
                return View(usersModel);
            }
            ViewBag.Message = "User Name or Password is invalid!";
            return View(usersModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userService?.Dispose();
                userMessagesService?.Dispose();
                db?.Dispose();
            }
        }
    }
}