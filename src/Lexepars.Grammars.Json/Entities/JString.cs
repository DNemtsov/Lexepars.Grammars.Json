using System.Globalization;
using System.Text.RegularExpressions;

namespace Lexepars.Grammars.Json.Entities
{
    public class JString : JValue
    {
        public string Value { get; }

        public JString(string value)
        {
            Value = value;
        }

        public static JString FromQuotedString(string quotation)
        {
            string result = quotation.Substring(1, quotation.Length - 2); //Remove leading and trailing quotation marks

            result = Regex.Replace(result, @"\\u[0-9a-fA-F]{4}",
                                   match => char.ConvertFromUtf32(int.Parse(match.Value.Replace("\\u", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture)));

            result = result
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\")
                .Replace("\\b", "\b")
                .Replace("\\f", "\f")
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t")
                .Replace("\\/", "/");

            return new JString(result);
        }

        public override bool Equals(object obj)
        {
            if (obj is JString str)
               return Value == str.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
