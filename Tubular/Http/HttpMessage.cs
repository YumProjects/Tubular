using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubular.Http
{
    public abstract class HttpMessage
    {
        // Body of the message
        public string body { get; set; } = "";

        // Internal set of headers
        Dictionary<string, string> _headers { get; set; } = new Dictionary<string, string>();

        // Returns headers as array to prevent write access
        public KeyValuePair<string, string>[] GetHeaders()
        {
            return _headers.ToArray();
        }

        // Sets or creates a header, prevents empty strings from being added
        public void SetHeader(string name, string value)
        {
            if (_headers.ContainsKey(name))
            {
                if (value == null || value == "") _headers.Remove(name);
                else _headers[name] = value;
            }
            else if (value != null && value != "")
            {
                _headers.Add(name, value);
            }
        }

        // Gets a header, if it's not in _headers then returns an empty string
        public string GetHeader(string name)
        {
            if (_headers.ContainsKey(name)) return _headers[name];
            else return "";
        }

        // Checks if a header exists
        public bool ContainsHeader(string name)
        {
            return _headers.ContainsKey(name);
        }

        public abstract void FromStream(Stream stream);
        public abstract void FromString(string str);
        public abstract override string ToString();
    }
}
