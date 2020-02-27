using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WissAppWebApi.Models
{
    public class UsersModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string School { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public RolesModel Roles { get; set; }
    }
}