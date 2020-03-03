using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Utils
{
    // String değerler üzerinde değişiklikler yapılmasını sağlayan utility class
    public static class StringUtil
    {
        public static string RemoveCommaFromEnd(string value)
        {
            char[] characters = new char[2];
            characters[0] = ' ';
            characters[1] = ',';
            string result;
            try
            {
                result = value.TrimEnd(characters);
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }

        public static string RemoveCharacterFromEnd(string value, char character)
        {
            char[] characters = new char[2];
            characters[0] = ' ';
            characters[1] = character;
            string result;
            try
            {
                result = value.TrimEnd(characters);
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }

        public static string RemoveStringFromEnd(string value, string stringToRemove)
        {
            string result;
            try
            {
                char[] characters = stringToRemove.ToCharArray();
                result = value.TrimEnd(characters);
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }

        public static string[] GetArrayByComma(string value)
        {
            string[] result;
            try
            {
                result = value.Split(',');
                if (result != null)
                {
                    if (result.Length > 0)
                    {
                        for (int i = 0; i < result.Length; i++)
                        {
                            result[i] = result[i].Trim();
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static string[] GetArrayByCharacter(string value, char character)
        {
            string[] result;
            try
            {
                result = value.Split(character);
                if (result != null)
                {
                    if (result.Length > 0)
                    {
                        for (int i = 0; i < result.Length; i++)
                        {
                            result[i] = result[i].Trim();
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static string[] GetArrayByString(string value, string _string)
        {
            string[] result;
            try
            {
                result = value.Split(new string[] { _string }, StringSplitOptions.None);
                if (result != null)
                {
                    if (result.Length > 0)
                    {
                        for (int i = 0; i < result.Length; i++)
                        {
                            result[i] = result[i].Trim();
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = null;                
            }
            return result;
        }

        public static string GetStringByCharacterAndArrayIndex(string value, char character, int index)
        {
            string result;
            try
            {
                result = value.Split(character)[index];
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        public static string RemoveHtmlTags(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            string result = new string(array, 0, arrayIndex);
            result = result.Replace("&nbsp;", " ").Replace("<br>", " ").Replace("<br />", " ").Replace("<br/>", " ").Replace("&amp;", "&");
            return result;
        }

        public static string ConvertTRtoEN(string value)
        {
            return value.Replace("ç", "c").Replace("Ç", "C")
                .Replace("ö", "o").Replace("Ö", "O")
                .Replace("ş", "s").Replace("Ş", "S")
                .Replace("ğ", "g").Replace("Ğ", "G")
                .Replace("ü", "u").Replace("Ü", "U")
                .Replace("ı", "i").Replace("İ", "I");
        }

        public static string GetStringBetween(string value, string start, string end)
        {
            int _start, _end;
            if (value.Contains(start) && value.Contains(end))
            {
                _start = value.IndexOf(start, 0) + start.Length;
                _end = value.IndexOf(end, _start);
                return value.Substring(_start, _end - _start);
            }
            else
            {
                return "";
            }
        }
    }
}
