using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using AppCore.Enums;

namespace AppCore.Utils
{
    public static class CacheUtil
    {
        public static object GetObject(string cacheName)
        {
            if (HttpContext.Current.Cache[cacheName] == null)
                return null;
            return HttpContext.Current.Cache[cacheName] as object;
        }

        public static void SetObject(string cacheName, object _object, int timeOutValue = 30, CacheEnum timeOut = CacheEnum.TimeOutInMinutes, CacheEnum expiration = CacheEnum.AbsoluteExpiration)
        {
            RemoveCache(cacheName);
            if (timeOut == CacheEnum.TimeOutInMinutes)
            {
                if (expiration == CacheEnum.AbsoluteExpiration)
                {
                    HttpContext.Current.Cache.Insert(cacheName, _object, null, DateTime.UtcNow.AddMinutes(timeOutValue), Cache.NoSlidingExpiration);
                }
                else
                {
                    HttpContext.Current.Cache.Insert(cacheName, _object, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, timeOutValue, 0));
                }
            }
            else if (timeOut == CacheEnum.TimeOutInSeconds)
            {
                if (expiration == CacheEnum.AbsoluteExpiration)
                {
                    HttpContext.Current.Cache.Insert(cacheName, _object, null, DateTime.UtcNow.AddSeconds(timeOutValue), Cache.NoSlidingExpiration);
                }
                else
                {
                    HttpContext.Current.Cache.Insert(cacheName, _object, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, timeOutValue));
                }
            }
        }

        public static void RemoveCache(string cacheName)
        {
            if (HttpContext.Current.Cache[cacheName] != null)
                HttpContext.Current.Cache.Remove(cacheName);
        }
    }
}
