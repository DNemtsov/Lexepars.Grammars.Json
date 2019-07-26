using System.Collections;
using System.Collections.Generic;

namespace Lexepars.Grammars.Json.Entities
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design.")]
    public class JObject : JValue, IReadOnlyDictionary<string, JValue>
    {
        private readonly IReadOnlyDictionary<string, JValue> _dictionary;

        public JObject(IReadOnlyDictionary<string, JValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public JValue this[string key] => _dictionary[key];

        public IEnumerable<string> Keys => _dictionary.Keys;

        public IEnumerable<JValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, JValue>> GetEnumerator() => _dictionary.GetEnumerator();

        public bool TryGetValue(string key, out JValue value) => _dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        public static JObject FromProperties(IEnumerable<KeyValuePair<JString, JValue>> pairs)
        {
            var result = new Dictionary<string, JValue>();

            foreach (var pair in pairs)
                result[pair.Key.Value] = pair.Value;

            return new JObject(result);
        }
    }
}
