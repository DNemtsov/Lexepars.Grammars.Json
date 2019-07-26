namespace Lexepars.Grammars.Json.Entities
{
    public class JBoolean : JValue
    {
        public bool Value { get; }

        public JBoolean(bool value)
        {
            Value = value;
        }

        public static readonly JBoolean True = new JBoolean(true);
        public static readonly JBoolean False = new JBoolean(false);

        public override bool Equals(object obj)
        {
            if (obj is JBoolean boo)
                return Value == boo.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
