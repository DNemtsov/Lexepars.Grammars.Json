using Lexepars.Grammars.Json.Entities;
using System;
using System.Collections.Generic;

namespace Lexepars.Grammars.Json
{
    public class JsonGrammar<TString, TNumber, TBoolean, TArray, TObject, TNull> : Grammar<JValue>
        where TString : JValue
        where TNumber : JValue
        where TBoolean : JValue
        where TArray : JValue
        where TObject : JValue
        where TNull : JValue
    {
        public GrammarRule<TBoolean> Boolean { get; } = new GrammarRule<TBoolean>();
        public GrammarRule<TNull> Null { get; } = new GrammarRule<TNull>();
        public GrammarRule<TNumber> Number { get; } = new GrammarRule<TNumber>();
        public GrammarRule<TString> String { get; } = new GrammarRule<TString>();
        public GrammarRule<TArray> Array { get; } = new GrammarRule<TArray>();
        public GrammarRule<TObject> Object { get; } = new GrammarRule<TObject>();
        public GrammarRule<JValue> JsonValue { get; } = new GrammarRule<JValue>();

        public JsonGrammar(
                Func<TBoolean> trueFab,
                Func<TBoolean> falseFab,
                Func<TNull> nullFab,
                Func<string, TNumber> numberFab,
                Func<string, TString> stringFab,
                Func<IList<JValue>, TArray> arrayFab,
                Func<IEnumerable<KeyValuePair<TString, JValue>>, TObject> objectFab,
                string name)
            : base(name)
        {
            Boolean.Rule = Choice(JsonLexer.True.Constant(trueFab()), JsonLexer.False.Constant(falseFab()));

            Null.Rule = JsonLexer.Null.Constant(nullFab());

            Number.Rule = JsonLexer.Number.BindLexeme(numberFab);

            String.Rule = JsonLexer.Quotation.BindLexeme(stringFab);

            Array.Rule =
                Between(JsonLexer.OpenArray.Kind(), ZeroOrMore(JsonValue, JsonLexer.Comma.Kind()), JsonLexer.CloseArray.Kind())
                .Bind(arrayFab);

            Object.Rule =
                Between(
                    JsonLexer.OpenDictionary.Kind(),
                    ZeroOrMore(NameValuePair(String, JsonLexer.Colon.Kind(), JsonValue), JsonLexer.Comma.Kind()),
                    JsonLexer.CloseDictionary.Kind())
                .Bind(objectFab);

            JsonValue.Rule = Choice<JValue>(Boolean, Null, Number, String, Object, Array);

            EntryRule = new GrammarRule<JValue> { Rule = OccupiesEntireInput(JsonValue) };
        }
    }



    public class JsonGrammar : JsonGrammar<JString, JNumber, JBoolean, JArray, JObject, JNull>
    {
        public JsonGrammar()
            : base(() => JBoolean.True,
                   () => JBoolean.False,
                   () => JNull.Null,
                   JNumber.FromString,
                   JString.FromQuotedString,
                   JArray.FromList,
                   JObject.FromProperties,
                  "JSON")
        {}
    }
}
