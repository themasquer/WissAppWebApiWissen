using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WissAppWebApi.Configs
{
    public static class UserConfig
    {
        private static List<string> _loggedOutUsers = null;
        private static int _loggedOutUsersMaximumCount = 100;
        private static int _loggedOutUsersRemoveCount = 10;

        static UserConfig()
        {
            if (_loggedOutUsers == null)
                _loggedOutUsers = new List<string>();
        }

        public static void AddLoggedOutUser(string userName)
        {
            if (_loggedOutUsers.Count > _loggedOutUsersMaximumCount)
            {
                _loggedOutUsers.RemoveRange(0, _loggedOutUsersRemoveCount);
            }
            if (!_loggedOutUsers.Contains(userName))
                _loggedOutUsers.Add(userName);
        }

        public static void RemoveLoggedOutUser(string userName)
        {
            if (_loggedOutUsers.Contains(userName))
                _loggedOutUsers.Remove(userName);
        }

        public static List<string> GetLoggedOutUsers()
        {
            return _loggedOutUsers;
        }
    }
}