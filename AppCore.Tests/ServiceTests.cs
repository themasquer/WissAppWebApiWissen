using System;
using System.Data.Entity;
using AppCore.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using WissAppEF.Contexts;
using WissAppEntities.Entities;

namespace AppCore.Tests
{
    [TestClass]
    public class ServiceTests
    {
        DbContext db = new WissAppContext();

        [TestMethod]
        public void ShouldGetUsers()
        {
            using (var service = new Service<Users>(db))
            {
                var entities = service.GetEntities();
                //Assert.AreNotEqual(0, entities.Count);
                entities.Count.ShouldBeGreaterThan(0);
            }
        }

        [TestMethod]
        public void ShouldAddUser()
        {
            var entity = new Users()
            {
                Id = 0,
                UserName = "test user",
                Password = "test pass",
                School = "test school",
                Location = "test location",
                BirthDate = DateTime.Now,
                Email = "test e-mail",
                Gender = "f",
                IsActive = true,
                RoleId = 2
            };
            using (var service = new Service<Users>(db))
            {
                service.AddEntity(entity);
            }
            entity.Id.ShouldNotBe(0);
        }

        [TestMethod]
        public void ShouldGetUser()
        {
            using (var service = new Service<Users>(db))
            {
                var entity = service.GetEntity(e => e.UserName == "leo");
                entity.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public void ShouldUpdateUser()
        {
            using (var service = new Service<Users>(db))
            {
                var entity = service.GetEntity(e => e.UserName == "leo");
                entity.ShouldNotBeNull();
                entity.UserName.ShouldBe("leo");
                entity.Password = "oel";
                service.UpdateEntity(entity);
                entity = service.GetEntity(e => e.UserName == "leo");
                entity.ShouldNotBeNull();
                entity.Password.ShouldBe("oel");
            }
        }

        [TestMethod]
        public void ShouldDeleteUser()
        {
            using (var service = new Service<Users>(db))
            {
                var entity = service.GetEntity(e => e.UserName == "leo");
                entity.ShouldNotBeNull();
                service.DeleteEntity(entity);
                entity = service.GetEntity(e => e.UserName == "leo" && e.IsDeleted == false);
                entity.ShouldBeNull();
            }
        }
    }
}
