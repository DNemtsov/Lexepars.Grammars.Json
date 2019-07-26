namespace Lexepars.Grammars.Json
{
    public class JsonLexer : Lexer
    {
        public JsonLexer()
            : base(
                  Skip(Whitespace),
                  Null,
                  True,
                  False,
                  Comma,
                  OpenArray,
                  CloseArray,
                  OpenDictionary,
                  CloseDictionary,
                  Colon,
                  Number,
                  Quotation)
        { }

        public static readonly MatchableTokenKind Whitespace = new PatternTokenKind("whitespace", @"\s+");
        public static readonly MatchableTokenKind Null = new KeywordTokenKind("null");
        public static readonly MatchableTokenKind True = new KeywordTokenKind("true");
        public static readonly MatchableTokenKind False = new KeywordTokenKind("false");
        public static readonly MatchableTokenKind Comma = new OperatorTokenKind(",");
        public static readonly MatchableTokenKind OpenArray = new OperatorTokenKind("[");
        public static readonly MatchableTokenKind CloseArray = new OperatorTokenKind("]");
        public static readonly MatchableTokenKind OpenDictionary = new OperatorTokenKind("{");
        public static readonly MatchableTokenKind CloseDictionary = new OperatorTokenKind("}");
        public static readonly MatchableTokenKind Colon = new OperatorTokenKind(":");
        public static readonly MatchableTokenKind Quotation = new PatternTokenKind("string", @"
            # Open quote:
            ""

            # Zero or more content characters:
            (
                      [^""\\]*             # Zero or more non-quote, non-slash characters.
                |     \\ [""\\bfnrt\/]     # One of: slash-quote   \\   \b   \f   \n   \r   \t   \/
                |     \\ u [0-9a-fA-F]{4}  # \u folowed by four hex digits
            )*

            # Close quote:
            ""
        ");
        public static readonly MatchableTokenKind Number = new PatternTokenKind("number", @"([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))?");
    }
}