using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Helpper
{
    public static class StringHelper
    {
        public const string VietNamePhoneRegex = @"^(0)(2[0-9][0-9]|3[2-9]|5[2|6|8|9]|7[0|6-9]|8[0-9]|9[0-9])[0-9]{7}$";
        public const string IdCardRegex = @"[0-9]{9}";
        public const string CitizenCardRegex = @"[0-9]{12}";
        public static string ToSnakeCase(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loại bỏ dấu tiếng việt
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToUnsign(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            return regex.Replace(s.Normalize(NormalizationForm.FormD),
                    String.Empty).Replace('\u0111', 'd')
                .Replace('\u0110', 'D');
        }

        /// <summary>
        /// Loại bỏ dấu, trim, xóa khoảng cách
        /// để tạo chuỗi tìm kiếm không dấu
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToFts(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            var strBuild = new StringBuilder();
            strBuild.Append(s.ConvertToUnsign());
            if (strBuild.Length == 0)
            {
                return strBuild.ToString();
            }
            return strBuild.ToString()
                .Trim()
                .ToLower()
                .RemoveMultiSpace();
        }

        /// <summary>
        /// Loại bỏ nhiều khoảng trắng
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveMultiSpace(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : Regex.Replace(s.Trim(), @"\s+", " ");
        }

        public static string ConvertFtsAndRemoveAllSpaces(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            var strBuild = new StringBuilder();
            strBuild.Append(s.ConvertToUnsign());
            if (strBuild.Length == 0)
            {
                return strBuild.ToString();
            }
            return strBuild.ToString()
                .Trim()
                .ToLower()
                .Replace(" ", "");
        }

        /// <summary>
        /// Tạo pattern where like sql
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LikeTextSearch(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (string.IsNullOrWhiteSpace(s)) return s;

            return $"%{s.ConvertToFts()}%";
        }

        public static string GetMimeType(this string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName).ToLower();
            return FileExtensionMapping.Mappings.TryGetValue(extension, out var mimeType) ? mimeType : "application/octet-stream";
        }

        public static string f_hex_to_string(this string v_hex_string)
        {
            if (string.IsNullOrEmpty(v_hex_string))
            {
                return string.Empty;
            }
            try
            {
                byte[] bytes = new byte[v_hex_string.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(v_hex_string.Substring(i * 2, 2), 16);
                }
                return Encoding.UTF8.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string CreateMd5(string input)
        {
            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);


            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();

        }

        public static string ConvertDecimalToStringVn(this decimal? value, bool isDefaultZero = false)
        {
            return value.HasValue ? value.Value.ConvertDecimalToStringVn() : (isDefaultZero ? "0" : string.Empty);
        }

        public static string ConvertDecimalToStringVn(this decimal value)
        {
            if (value == 0) return "0";
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            return value.ToString("#,##", cul.NumberFormat);
        }

        public static string ConvertToBase64(this string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static string ToCapitalize(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            str = str.Trim().RemoveMultiSpace();
            var lst = str.Split(" ")
                .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower());
            return string.Join(" ", lst);
        }

        public static string FileNameExport(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "" + DateTime.Now.ToString("dd/MM/yyy_HH:ss");

            return $"{s}_{DateTime.Now:dd/MM/yyy_HH:ss}";
        }

        public static string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            input = input.Replace("\u0000", string.Empty).Trim();
            input = new string(input.Where(c => !char.IsControl(c)).ToArray());
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input));
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            text = text.Trim().Normalize(NormalizationForm.FormD); // Chuẩn hóa Unicode
            StringBuilder sb = new StringBuilder();

            foreach (char c in text)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}