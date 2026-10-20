using System;

namespace AlRowad_ERP.Core
{
    /// <summary>
    /// المحرك المركزي لتفقيط المبالغ المالية باللغة العربية - نظام الرواد ERP
    /// </summary>
    public static class CurrencyTafqeetHelper
    {
        private static readonly string[] Ones = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        private static readonly string[] Tens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        private static readonly string[] Hundreds = { "", "مائة", "مائتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };

        /// <summary>
        /// تحويل المبلغ الرقمي إلى نص عربي محاسبي موحد
        /// </summary>
        public static string ConvertToWords(decimal amount, string mainCurrency = "ريال", string subCurrency = "فلس")
        {
            if (amount == 0)
                return $"صفر {mainCurrency}";

            amount = Math.Abs(amount);
            long integerPart = (long)Math.Truncate(amount);
            int fractionPart = (int)Math.Round((amount - integerPart) * 100);

            string result = "";

            if (integerPart > 0)
            {
                result = ConvertWholeNumber(integerPart) + " " + mainCurrency;
            }

            if (fractionPart > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " و";

                result += ConvertWholeNumber(fractionPart) + " " + subCurrency;
            }

            return $"فقط {result} لا غير";
        }

        private static string ConvertWholeNumber(long number)
        {
            if (number == 0) return "";

            if (number >= 1000000000)
            {
                long billions = number / 1000000000;
                long remainder = number % 1000000000;
                string billionText = FormatGroup(billions, "مليار", "ملياران", "مليارات", "مليار");
                return billionText + (remainder > 0 ? " و" + ConvertWholeNumber(remainder) : "");
            }

            if (number >= 1000000)
            {
                long millions = number / 1000000;
                long remainder = number % 1000000;
                string millionText = FormatGroup(millions, "مليون", "مليونان", "ملايين", "مليون");
                return millionText + (remainder > 0 ? " و" + ConvertWholeNumber(remainder) : "");
            }

            if (number >= 1000)
            {
                long thousands = number / 1000;
                long remainder = number % 1000;
                string thousandText = FormatGroup(thousands, "ألف", "ألفان", "آلاف", "ألف");
                return thousandText + (remainder > 0 ? " و" + ConvertWholeNumber(remainder) : "");
            }

            return ConvertLessThanOneThousand((int)number);
        }

        private static string FormatGroup(long count, string singular, string dual, string plural, string defaultNoun)
        {
            if (count == 1) return singular;
            if (count == 2) return dual;
            if (count >= 3 && count <= 10) return ConvertLessThanOneThousand((int)count) + " " + plural;
            return ConvertLessThanOneThousand((int)count) + " " + defaultNoun;
        }

        private static string ConvertLessThanOneThousand(int number)
        {
            string result = "";

            int hundreds = number / 100;
            int remainder = number % 100;

            if (hundreds > 0)
            {
                result = Hundreds[hundreds];
            }

            if (remainder > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " و";

                if (remainder < 20)
                {
                    result += Ones[remainder];
                }
                else
                {
                    int ones = remainder % 10;
                    int tens = remainder / 10;

                    if (ones > 0)
                        result += Ones[ones] + " و" + Tens[tens];
                    else
                        result += Tens[tens];
                }
            }

            return result;
        }
    }
}