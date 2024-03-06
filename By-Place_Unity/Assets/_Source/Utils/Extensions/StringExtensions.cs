using System.Linq;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        public static string CamelCaseToUpperCaseWithSpaces(this string text) => string.Concat(text.Select(
            (x, i) => i > 0 
                ? char.IsUpper(x) 
                    ? $" {x}" 
                    : x.ToString()
                : char.ToUpper(x).ToString()));
    }
}