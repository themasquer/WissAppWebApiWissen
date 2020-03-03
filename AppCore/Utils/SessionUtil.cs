using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppCore.Utils
{
    // Session işlemlerinin dinamik tipe göre gerçekleştirildiği utility class
    public static class SessionUtil<T> where T : class
    {
        public static List<T> GetList(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName] as List<T>;
        }
        public static IQueryable<T> GetQuery(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName] as IQueryable<T>;
        }
        public static T GetItem(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName] as T;
        }
        public static string GetString(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName].ToString();
        }
        public static void SetList(string sessionName, List<T> list)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = list;
        }
        public static void SetQuery(string sessionName, IQueryable<T> query)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = query;
        }
        public static void SetItem(string sessionName, T item)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = item;
        }
        public static void SetString(string sessionName, string value)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = value;
        }
        public static void RemoveSession(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] != null)
                HttpContext.Current.Session.Remove(sessionName);
        }
    }

    // Session işlemlerinin obje üzerinden gerçekleştirildiği utility class
    public static class SessionUtil
    {
        public static object GetObject(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName] as object;
        }
        public static string GetString(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
                return null;
            return HttpContext.Current.Session[sessionName].ToString();
        }
        public static void SetObject(string sessionName, object _object)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = _object;
        }
        public static void SetString(string sessionName, string value)
        {
            RemoveSession(sessionName);
            HttpContext.Current.Session[sessionName] = value;
        }
        public static void RemoveSession(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] != null)
                HttpContext.Current.Session.Remove(sessionName);
        }
    }
}
