using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.API.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this String String)
        {
            return String.IsNullOrEmpty(String);
        }
    }
}