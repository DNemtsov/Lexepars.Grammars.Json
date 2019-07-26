using System.Collections;
using System.Collections.Generic;

namespace Lexepars.Grammars.Json.Entities
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design.")]
    public class JArray : JValue, IReadOnlyList<JValue>
    {
        private readonly IList<JValue> _items;

        public JArray(IList<JValue> items)
        {
            _items = items;
        }

        public static JArray FromList(IList<JValue> items) => new JArray(items);

        public JValue this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<JValue> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
