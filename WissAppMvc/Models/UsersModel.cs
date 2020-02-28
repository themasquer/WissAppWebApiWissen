using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WissAppMvc.Models
{
    public class UsersModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool Broadcast { get; set; } = false;
        public int MessageCount { get; set; }
    }
}