using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Helpper
{
    public static class SqlFunctionHelper
    {
        public static string ConvertToFts(string input_text)
        {
            throw new NotImplementedException();
        }
        public static int ContainArray(string value, string compare)
        {
            throw new NotImplementedException();
        }

        public static int DateToNumber(DateTime? input_text)
        {
            throw new NotImplementedException();
        }

        public static string Unaccent(string input) => throw new NotSupportedException("This function is only for use with PostgreSQL.");
    }
}