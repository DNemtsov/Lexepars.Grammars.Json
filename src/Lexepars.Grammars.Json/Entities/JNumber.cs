using System.Globalization;

namespace Lexepars.Grammars.Json.Entities
{
    public class JNumber : JValue
    {
        public decimal Value { get; }

        public JNumber(decimal value)
        {
            Value = value;
        }

        public static JNumber FromString(string str)
        {
            return new JNumber(decimal.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture));
        }

        public override bool Equals(object obj)
        {
            if (obj is JNumber number)
                return Value == number.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
