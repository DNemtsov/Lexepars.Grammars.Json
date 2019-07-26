namespace Lexepars.Grammars.Json.Entities
{
    public class JNull : JValue
    {
        private JNull()
        {
        }

        public static readonly JNull Null = new JNull();

        public override bool Equals(object obj)
        {
            return obj is JNull;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
