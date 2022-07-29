using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StreamingReplacer;

public class JsonArrayStreamer<T> : IEnumerable<T>, IDisposable
{
    private readonly LineEnumerator _enumerator;

    public JsonArrayStreamer(JsonReader reader)
    {
        _enumerator = new LineEnumerator(reader);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        _enumerator.Dispose();
        GC.SuppressFinalize(this);
    }

    private class LineEnumerator : IEnumerator<T>
    {
        private readonly JsonReader _reader;

        public LineEnumerator(JsonReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (true)
            {
                var line = _reader.Read();

                if (line is false) return 
                    false;

                if (_reader.TokenType != JsonToken.StartObject)
                    continue;

                var jObject = JObject.Load(_reader);
                Current = jObject.ToObject<T>();

                return true;
            }
        }

        public void Reset()
        {
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;
    }
}