using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Utils
{
    // Sayı değerinin yazı olarak formatlı bir şekilde dönüştürülmesini sağlayan utility class
    public static class NumberUtil
    {
        // Bir katrilyara kadar sayıları destekler.
        public static string ConvertLongToStringTR(long number, bool adjoint = true, bool returnZero = true)
        {
            if (number > 1000000000000000000)
                return "";
            if (number == 0)
                return returnZero == true ? "SIFIR" : "";
            string result = "";
            try
            {
                string sNumber = number.ToString();
                if (number < 0)
                {
                    result = "EKSİ ";
                    sNumber = sNumber.Replace("-", "");
                }
                string[] ones = null;
                string[] tens = null;
                string[] thousands = null;
                if (adjoint)
                {
                    ones = new string[] { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
                    tens = new string[] { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
                    thousands = new string[] { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" };
                }
                else
                {
                    ones = new string[] { "", "BİR ", "İKİ ", "ÜÇ ", "DÖRT ", "BEŞ ", "ALTI ", "YEDİ ", "SEKİZ ", "DOKUZ " };
                    tens = new string[] { "", "ON ", "YİRMİ ", "OTUZ ", "KIRK ", "ELLİ ", "ALTMIŞ ", "YETMİŞ ", "SEKSEN ", "DOKSAN " };
                    thousands = new string[] { "KATRİLYON ", "TRİLYON ", "MİLYAR ", "MİLYON ", "BİN ", "" };
                }
                int groupCount = 6;
                sNumber = sNumber.PadLeft(groupCount * 3, '0');
                string groupValue;
                for (int i = 0; i < groupCount * 3; i += 3)
                {
                    groupValue = "";
                    if (sNumber.Substring(i, 1) != "0")
                    {
                        if (adjoint)
                            groupValue += ones[Convert.ToInt32(sNumber.Substring(i, 1))] + "YÜZ";
                        else
                            groupValue += ones[Convert.ToInt32(sNumber.Substring(i, 1))] + "YÜZ ";
                    }
                    if (groupValue.Trim() == "BİRYÜZ" || groupValue.Trim() == "BİR YÜZ")
                        groupValue = adjoint == true ? "YÜZ" : "YÜZ ";
                    groupValue += tens[Convert.ToInt32(sNumber.Substring(i + 1, 1))];
                    groupValue += ones[Convert.ToInt32(sNumber.Substring(i + 2, 1))];
                    if (groupValue != "")
                        groupValue += thousands[i / 3];
                    if (groupValue.Trim() == "BİRBİN" || groupValue.Trim() == "BİR BİN")
                        groupValue = adjoint == true ? "BİN" : "BİN ";
                    result += groupValue;
                }
                result = result.Trim();
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }

        // On trilyona kadar sayıları destekler. On trilyondan önceki sayıların ondalık noktasından sonraki sayıları iki hane olarak yuvarlar.
        public static string ConvertDoubleToStringTR(double number, string decimalPoint = ",", bool adjoint = true, bool returnZero = true, bool returnDecimalZero = false, bool decimalFormat = true)
        {
            if (number > 10000000000000.0)
                return "";
            if (decimalPoint.Trim().Equals(""))
                return "";
            if (!(decimalPoint.Trim().Equals(",") || decimalPoint.Trim().Equals(".")))
                return "";
            if (number == 0)
                return returnZero == true ? "SIFIR" : "";
            string result = "";
            try
            {
                string sNumber = number.ToString("F12").TrimEnd(new char[] { '0' }).Replace('.', ',');
                if (number < 0)
                {
                    result = "EKSİ ";
                    sNumber = sNumber.Replace("-", "");
                }
                string wholePart = sNumber;
                string decimalPart = "0";
                string decimalFormatText = "";
                if (sNumber.Contains(","))
                {
                    wholePart = sNumber.Substring(0, sNumber.IndexOf(','));
                    decimalPart = sNumber.Substring(sNumber.IndexOf(',') + 1);
                    if (decimalPart.Equals(""))
                        decimalPart = "0";
                }
                if (decimalPart.Length > 12)
                    return "";
                if (wholePart.Equals("0") && decimalFormat)
                {
                    switch (decimalPart.Length)
                    {
                        case 1:
                            decimalFormatText = adjoint == true ? "ONDA" : "ONDA ";
                            break;
                        case 2:
                            decimalFormatText = adjoint == true ? "YÜZDE" : "YÜZDE ";
                            break;
                        case 3:
                            decimalFormatText = adjoint == true ? "BİNDE" : "BİNDE ";
                            break;
                        case 4:
                            decimalFormatText = adjoint == true ? "ONBİNDE" : "ON BİNDE ";
                            break;
                        case 5:
                            decimalFormatText = adjoint == true ? "YÜZBİNDE" : "YÜZ BİNDE ";
                            break;
                        case 6:
                            decimalFormatText = adjoint == true ? "MİLYONDA" : "MİLYONDA ";
                            break;
                        case 7:
                            decimalFormatText = adjoint == true ? "ONMİLYONDA" : "ON MİLYONDA ";
                            break;
                        case 8:
                            decimalFormatText = adjoint == true ? "YÜZMİLYONDA" : "YÜZ MİLYONDA ";
                            break;
                        case 9:
                            decimalFormatText = adjoint == true ? "MİLYARDA" : "MİLYARDA ";
                            break;
                        case 10:
                            decimalFormatText = adjoint == true ? "ONMİLYARDA" : "ON MİLYARDA ";
                            break;
                        case 11:
                            decimalFormatText = adjoint == true ? "YÜZMİLYARDA" : "YÜZ MİLYARDA ";
                            break;
                        case 12:
                            decimalFormatText = adjoint == true ? "TRİLYONDA" : "TRİLYONDA ";
                            break;
                        default:
                            decimalFormatText = "";
                            break;
                    }
                }
                string[] ones = null;
                string[] tens = null;
                string[] thousands = null;
                int groupCount = 6;
                string groupValue;
                if (adjoint)
                {
                    ones = new string[] { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
                    tens = new string[] { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
                    thousands = new string[] { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" };
                }
                else
                {
                    ones = new string[] { "", "BİR ", "İKİ ", "ÜÇ ", "DÖRT ", "BEŞ ", "ALTI ", "YEDİ ", "SEKİZ ", "DOKUZ " };
                    tens = new string[] { "", "ON ", "YİRMİ ", "OTUZ ", "KIRK ", "ELLİ ", "ALTMIŞ ", "YETMİŞ ", "SEKSEN ", "DOKSAN " };
                    thousands = new string[] { "KATRİLYON ", "TRİLYON ", "MİLYAR ", "MİLYON ", "BİN ", "" };
                }
                if (wholePart.Equals("0"))
                {
                    if (decimalFormat)
                        result += decimalFormatText;
                    else
                        result += "SIFIR";
                }
                else
                {
                    wholePart = wholePart.PadLeft(groupCount * 3, '0');
                    for (int i = 0; i < groupCount * 3; i += 3)
                    {
                        groupValue = "";
                        if (wholePart.Substring(i, 1) != "0")
                        {
                            if (adjoint)
                                groupValue += ones[Convert.ToInt32(wholePart.Substring(i, 1))] + "YÜZ";
                            else
                                groupValue += ones[Convert.ToInt32(wholePart.Substring(i, 1))] + "YÜZ ";
                        }
                        if (groupValue.Trim() == "BİRYÜZ" || groupValue.Trim() == "BİR YÜZ")
                            groupValue = adjoint == true ? "YÜZ" : "YÜZ ";
                        groupValue += tens[Convert.ToInt32(wholePart.Substring(i + 1, 1))];
                        groupValue += ones[Convert.ToInt32(wholePart.Substring(i + 2, 1))];
                        if (groupValue != "")
                            groupValue += thousands[i / 3];
                        if (groupValue.Trim() == "BİRBİN" || groupValue.Trim() == "BİR BİN")
                            groupValue = adjoint == true ? "BİN" : "BİN ";
                        result += groupValue;
                    }
                    if (Convert.ToInt64(wholePart) != 0)
                    {
                        result = result.Trim();
                    }
                    else
                    {
                        result = "";
                    }
                }
                if (decimalPart.Equals("0") && !returnDecimalZero)
                    return result;
                if (decimalFormatText.Equals(""))
                {
                    if (decimalPoint.Trim().Equals(","))
                    {
                        if (adjoint)
                            result += "VİRGÜL";
                        else
                            result += " VİRGÜL ";
                    }
                    else
                    {
                        if (adjoint)
                            result += "NOKTA";
                        else
                            result += " NOKTA ";
                    }
                    bool zeroFound = true;
                    for (int i = 0; i < decimalPart.Length && zeroFound; i++)
                    {
                        if (decimalPart.Substring(i, 1).Equals("0"))
                        {
                            if (adjoint)
                                result += "SIFIR";
                            else
                                result += "SIFIR ";
                            zeroFound = false;
                            if ((i + 1) < decimalPart.Length)
                            {
                                if (decimalPart.Substring((i + 1), 1).Equals("0"))
                                    zeroFound = true;
                            }
                        }
                        else
                        {
                            zeroFound = false;
                        }
                    }
                }
                decimalPart = decimalPart.PadLeft(groupCount * 3, '0');
                for (int i = 0; i < groupCount * 3; i += 3)
                {
                    groupValue = "";
                    if (decimalPart.Substring(i, 1) != "0")
                    {
                        if (adjoint)
                            groupValue += ones[Convert.ToInt32(decimalPart.Substring(i, 1))] + "YÜZ";
                        else
                            groupValue += ones[Convert.ToInt32(decimalPart.Substring(i, 1))] + "YÜZ ";
                    }
                    if (groupValue.Trim() == "BİRYÜZ" || groupValue.Trim() == "BİR YÜZ")
                        groupValue = adjoint == true ? "YÜZ" : "YÜZ ";
                    groupValue += tens[Convert.ToInt32(decimalPart.Substring(i + 1, 1))];
                    groupValue += ones[Convert.ToInt32(decimalPart.Substring(i + 2, 1))];
                    if (groupValue != "")
                        groupValue += thousands[i / 3];
                    if (groupValue.Trim() == "BİRBİN" || groupValue.Trim() == "BİR BİN")
                        groupValue = adjoint == true ? "BİN" : "BİN ";
                    result += groupValue;
                }
                result = result.Trim();
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }

        public static string ConvertPercentageToStringTR(double number, string decimalPoint = ".", bool adjoint = true, bool returnZero = true, bool returnDecimalZero = false)
        {
            if (number < 0.0)
                return "";
            if (number == 0.0 && returnZero)
            {
                if (adjoint)
                {
                    return "YÜZDESIFIR";
                }
                else
                {
                    return "YÜZDE SIFIR";
                }
            }
            bool decimalFormat;
            string result = "";
            double nNumber;
            if (number >= 0.0 && number < 1.0)
            {
                decimalFormat = true;
                nNumber = number / 100;
            }
            else
            {
                decimalFormat = false;
                nNumber = number;
                if (adjoint)
                    result = "YÜZDE";
                else
                    result = "YÜZDE ";
            }
            result += ConvertDoubleToStringTR(nNumber, decimalPoint, adjoint, returnZero, returnDecimalZero, decimalFormat);
            return result;
        }
    }
}
