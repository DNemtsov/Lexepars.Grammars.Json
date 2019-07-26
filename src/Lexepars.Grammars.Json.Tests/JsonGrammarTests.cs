using Lexepars.Grammars.Json.Entities;
using Lexepars.TestFixtures;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Lexepars.Grammars.Json.Tests
{
    public class JsonGrammarTests
    {
        static IEnumerable<Token> Tokenize(string input) => new JsonLexer().Tokenize(input);
        static IEnumerable<Token> TokenizeLined(string input) => new JsonLinedLexer().Tokenize(new StringReader(input));
        static readonly JsonGrammar Json = new JsonGrammar();

        [Fact]
        public void ParsesTrueLexeme()
        {
            var tokens = Tokenize("true");

            Json.Parses(tokens).WithValue(JBoolean.True);
        }

        [Fact]
        public void ParsesFalseLexeme()
        {
            var tokens = Tokenize("false");

            Json.Parses(tokens).WithValue(JBoolean.False);
        }

        [Fact]
        public void ParsesNullLexeme()
        {
            var tokens = Tokenize("null");

            Json.Parses(tokens).WithValue(JNull.Null);
        }

        [Fact]
        public void ParsesNumbers()
        {
            var tokens = Tokenize("10.123E-11");

            Json.Parses(tokens).WithValue(new JNumber(10.123E-11m));
        }

        [Fact]
        public void ParsesQuotations()
        {
            var empty = Tokenize("\"\"");
            var filled = Tokenize("\"abc \\\" \\\\ \\/ \\b \\f \\n \\r \\t \\u263a def\"");
            var expected = new JString("abc \" \\ / \b \f \n \r \t ☺ def");

            Json.Parses(empty).WithValue(new JString(""));
            Json.Parses(filled).WithValue(expected);
        }

        [Fact]
        public void ParsesArrays()
        {
            var empty = Tokenize("[]");
            var filled = Tokenize("[0, 1, 2]");

            ((JArray)Json.Parses(empty).ParsedValue).ShouldBeEmpty();

            ((JArray)Json.Parses(filled).ParsedValue).ShouldBe(new[] { new JNumber(0m), new JNumber(1m), new JNumber(2m) });
        }

        [Fact]
        public void ParsesDictionaries()
        {
            var empty = Tokenize("{}");

            Json.Parses(empty).WithValue(value => ((JObject)value).Count.ShouldBe(0));

            var filled = "{\"zero\" \n : \n 0, \"one\" \n \n\n: 1, \"two\" \n\n : 2}";

            foreach (var tokens in new[] { Tokenize(filled), TokenizeLined(filled) })
                Json.Parses(tokens).WithValue(value =>
                {
                    var dictionary = (JObject) value;

                    dictionary["zero"].ShouldBe(new JNumber(0m));
                    dictionary["one"].ShouldBe(new JNumber(1m));
                    dictionary["two"].ShouldBe(new JNumber(2m));
                });
        }

        [Fact]
        public void ParsesComplexJsonValues()
        {
            const string complex = @"

                {
                    ""numbers"" : [10, 20, 30],
                    ""window"":
                    {
                        ""title"": ""Sample Widget"",
                        ""parent"": null,
                        ""maximized"": true,
                        ""transparent"": false
                    }
                }

            ";

            foreach(var tokens in new []{ Tokenize(complex), TokenizeLined(complex) })
                Json.Parses(tokens).WithValue(value =>
                {
                    var json = (JObject)value;
                    ((JArray)json["numbers"]).ShouldBe(new[] {new JNumber(10m), new JNumber(20m), new JNumber(30m)});

                    var window = (JObject) json["window"];
                    window["title"].ShouldBe(new JString("Sample Widget"));
                    window["parent"].ShouldBe(JNull.Null);
                    window["maximized"].ShouldBe(JBoolean.True);
                    window["transparent"].ShouldBe(JBoolean.False);
                });
        }

        [Fact]
        public void RequiresEndOfInputAfterFirstValidJsonValue()
        {
            Json.FailsToParse(Tokenize("true $garbage$")).LeavingUnparsedTokens("$garbage$").WithMessage("(1, 6): end of input expected");
            Json.FailsToParse(Tokenize("10.123E-11  $garbage$")).LeavingUnparsedTokens("$garbage$").WithMessage("(1, 13): end of input expected");
            Json.FailsToParse(Tokenize("\"garbage\" $garbage$")).LeavingUnparsedTokens("$garbage$").WithMessage("(1, 11): end of input expected");
            Json.FailsToParse(Tokenize("[0, 1, 2] $garbage$")).LeavingUnparsedTokens("$garbage$").WithMessage("(1, 11): end of input expected");
            Json.FailsToParse(Tokenize("{\"zero\" : 0} $garbage$")).LeavingUnparsedTokens("$garbage$").WithMessage("(1, 14): end of input expected");
        }
    }
}