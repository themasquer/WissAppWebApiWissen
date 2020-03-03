using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Utils
{
    // Para değerinin yazı olarak formatlı bir şekilde dönüştürülmesini sağlayan utility class
    public static class MoneyUtil
    {
        // On trilyona kadar sayıları destekler. On trilyondan önceki sayıların ondalık noktasından sonraki sayıları iki hane olarak yuvarlar.
        public static string ConvertMoneyToStringTR(double amount, string currency = "TL", string decimalPoint = ",", bool adjoint = true, bool returnZero = true)
        {
            if (amount > 10000000000000.0)
                return "";
            if (currency.Trim().Equals(""))
                return "";
            if (amount == 0)
                return returnZero == true ? "SIFIR " + currency : "";
            string result = "";
            try
            {
                string sAmount = amount.ToString("F2").Replace('.', ',');
                string wholePart = sAmount.Substring(0, sAmount.IndexOf(','));
                string decimalPart = sAmount.Substring(sAmount.IndexOf(',') + 1, 2);
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
                wholePart = wholePart.PadLeft(groupCount * 3, '0');
                string groupValue;
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
                    if (currency.Trim().ToUpper().Equals("TL"))
                    {
                        if (adjoint)
                            result += " " + currency;
                        else
                            result += currency;
                    }
                    result = result.Trim();
                }
                else
                {
                    result = "";
                }
                if (Convert.ToInt64(decimalPart) != 0)
                {
                    if (currency.Trim().ToUpper().Equals("TL"))
                    {
                        result += " ";
                    }
                    else
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
                    }
                    if (decimalPart.Substring(0, 1) != "0")
                        result += tens[Convert.ToInt32(decimalPart.Substring(0, 1))];
                    if (decimalPart.Substring(1, 1) != "0")
                        result += ones[Convert.ToInt32(decimalPart.Substring(1, 1))];
                    result = result.Trim();
                    if (currency.Trim().ToUpper().Equals("TL"))
                        result += " Kr.";
                    else
                        result += " " + currency;
                }
                else
                {
                    if (!currency.Trim().ToUpper().Equals("TL"))
                        result += " " + currency;
                }
            }
            catch (Exception exc)
            {
                result = "";
            }
            return result;
        }
    }
}
