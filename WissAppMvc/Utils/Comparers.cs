using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WissAppMvc.Models;

namespace WissAppMvc.Utils
{
    public class UsersModelComparer : IEqualityComparer<UsersModel>
    {
        public bool Equals(UsersModel x, UsersModel y)
        {
            return x.UserId == y.UserId;
        }

        public int GetHashCode(UsersModel obj)
        {
            return obj.UserId.GetHashCode();
        }
    }
}