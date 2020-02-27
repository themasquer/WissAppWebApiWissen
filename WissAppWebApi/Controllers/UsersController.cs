using AppCore.Services;
using AppCore.Services.Base;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WissAppEF.Contexts;
using WissAppEntities.Entities;
using WissAppWebApi.Attributes;
using WissAppWebApi.Configs;
using WissAppWebApi.Models;

namespace WissAppWebApi.Controllers
{
    [RoutePrefix("api/Users")]
    //[ClaimsAuthorize(ClaimType = "role", ClaimValue = "admin,user")]
    [ClaimsAuthorize(ClaimType = "role", ClaimValue = "admin")]
    public class UsersController : ApiController
    {
        DbContext db;
        ServiceBase<Users> userService;

        public UsersController()
        {
            db = new WissAppContext();
            userService = new Service<Users>(db);
        }

        //[AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var entities = userService.GetEntities(e => e.IsDeleted == false);
                //var model = entities.Select(e => new UsersModel()
                //{
                //    Id = e.Id,
                //    RoleId = e.RoleId,
                //    UserName = e.UserName,
                //    Password = e.Password,
                //    Email = e.Email,
                //    School = e.School,
                //    Location = e.Location,
                //    BirthDate = e.BirthDate,
                //    Gender = e.Gender,
                //    IsActive = e.IsActive
                //}).ToList();
                //var model = Mapping.mapper.Map<List<Users>, List<UsersModel>>(entities);
                //var model = Mapping.mapper.Map<List<UsersModel>>(entities);
                var model = userService.GetEntityQuery(e => e.IsDeleted == false).ProjectTo<UsersModel>(MappingConfig.mapperConfiguration).ToList();
                return Ok(model);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                var entity = userService.GetEntity(e => e.Id == id && e.IsDeleted == false);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult Post(UsersModel usersModel)
        {
            try
            {
                var entity = Mapping.mapper.Map<Users>(usersModel);
                entity.IsActive = true;
                userService.AddEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult Put(UsersModel usersModel)
        {
            try
            {
                var entity = userService.GetEntity(e => e.Id == usersModel.Id && e.IsDeleted == false);
                entity.BirthDate = usersModel.BirthDate;
                entity.Email = usersModel.Email;
                entity.Gender = usersModel.Gender;
                entity.IsActive = usersModel.IsActive;
                entity.Location = usersModel.Location;
                entity.Password = usersModel.Password;
                entity.RoleId = usersModel.RoleId;
                entity.School = usersModel.School;
                entity.UserName = usersModel.UserName;
                userService.UpdateEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entity = userService.GetEntity(e => e.Id == id && e.IsDeleted == false);
                userService.DeleteEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception exc)
            {

                return BadRequest();
            }
        }

        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var entities = userService.GetEntities();
                var resultEntities = JsonConvert.SerializeObject(entities, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Ok(JsonConvert.DeserializeObject(resultEntities));
            }
            catch (Exception exc)
            {

                return BadRequest();
            }
        }

        [Route("Logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            var principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal.Identity.IsAuthenticated)
            {
                UserConfig.AddLoggedOutUser(principal.FindFirst(e => e.Type == "user").Value);
                return Ok("User logged out.");
            }
            return BadRequest("User didn't login.");
        }

        [Route("LoggedoutUsers")]
        //[AllowAnonymous]
        [HttpGet]
        public IHttpActionResult LoggedoutUsers()
        {
            return Ok(UserConfig.GetLoggedOutUsers());
        }
    }
}
