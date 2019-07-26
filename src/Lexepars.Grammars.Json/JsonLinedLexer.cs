namespace Lexepars.Grammars.Json
{
    public class JsonLinedLexer : LinedLexer
    {
        public JsonLinedLexer()
            : base(Skip(JsonLexer.Whitespace), JsonLexer.Null, JsonLexer.True, JsonLexer.False, JsonLexer.Comma, JsonLexer.OpenArray, JsonLexer.CloseArray, JsonLexer.OpenDictionary, JsonLexer.CloseDictionary, JsonLexer.Colon,JsonLexer.Number, JsonLexer.Quotation)
        { }
    }
}
