using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Utils
{
    // Tarihler için DateTime - string dönüşümlerinin yapıldığı utility class
    public static class DateTimeUtil
    {
        public static DateTime? GetDateFromStringWithTRformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("."))
                return null;
            if (date.Split('.').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('.')[2]), Convert.ToInt32(date.Split('.')[1]), Convert.ToInt32(date.Split('.')[0]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithTRformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("."))
                return null;
            if (date.Split('.').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('.')[2]), Convert.ToInt32(date.Split('.')[1]), Convert.ToInt32(date.Split('.')[0]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithTRformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString();
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return day + "." + month + "." + year + " " + hour + ":" + minute + ":" + second;
            return day + "." + month + "." + year;
        }

        public static DateTime? GetDateFromStringWithENformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("/"))
                return null;
            if (date.Split('/').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('/')[2]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(date.Split('/')[1]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithENformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("/"))
                return null;
            if (date.Split('/').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('/')[2]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(date.Split('/')[1]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithENformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString();
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return month + "/" + day + "/" + year + " " + hour + ":" + minute + ":" + second;
            return month + "/" + day + "/" + year;
        }

        public static DateTime? GetDateFromStringWithSQLformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("-"))
                return null;
            if (date.Split('-').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithSQLformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("-"))
                return null;
            if (date.Split('-').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithSQLformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString();
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
            return year + "-" + month + "-" + day;
        }

        public static string GetStringFromDateWithSQLformat(DateTime dateTime, string dateTimeSeperator)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString();
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            return year + "-" + month + "-" + day + dateTimeSeperator + hour + ":" + minute + ":" + second;
        }
    }
}
