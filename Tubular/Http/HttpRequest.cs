using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tubular.Http
{
    public class HttpRequest : HttpMessage
    {
        // Major parts of the HTTP reuquest
        public string method { get; set; } = "";
        public string url { get; set; } = "";

        // Standard constructor for method and url
        public HttpRequest(string method, string url)
        {
            this.method = method;
            this.url = url;
        }

        // Creates an HttpRequest instance from a HTTP stream
        public override void FromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);

            // Read the standard first line and create the request instance
            string nextLine = reader.ReadLine();
            method = nextLine.Split(' ')[0];
            url = nextLine.Split(' ')[1];

            // Parse header until an empty line is found
            do
            {
                nextLine = reader.ReadLine();
                if (nextLine != "")
                {
                    string name = nextLine.Split(':')[0].Trim();
                    string value = nextLine.Split(':')[1].Trim();
                    SetHeader(name, value);
                }
            } while (nextLine != "");

            /* If the header contains a Content-Length then save that
               many bytes as the body */
            if (ContainsHeader("Content-Length"))
            {
                int length = int.Parse(GetHeader("Content-Length"));
                char[] buffer = new char[length];
                reader.ReadBlock(buffer, 0, length);
                body = new string(buffer);
            }
        }

        // Creates an HttpRequest instance from a string
        public override void FromString(string str)
        {
            // Split the request into lines
            string[] lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Read the standard first line
            method = lines[0].Split(' ')[0];
            url = lines[0].Split(' ')[1];

            // Parse headers until an empty line is found
            int line;
            for(line = 1; line < lines.Length && lines[line] != ""; line++)
            {
                string name = lines[line].Split(':')[0].Trim();
                string value = lines[line].Split(':')[1].Trim();
                SetHeader(name, value);
            }

            // Copy the remaining lines as the body
            for(; line < lines.Length; line++)
            {
                body += lines[line];
            }
        }

        // Converts this HttpRequest instance into a string
        public override string ToString()
        {
            // Start with the standard request line
            string result = method + " " + url + " HTTP/1.1\r\n";
            
            // Add all headers
            foreach (KeyValuePair<string, string> header in GetHeaders())
            {
                result += header.Key + ": " + header.Value + "\r\n";
            }

            // If the request has a body then add the required empty line + body
            if (body.Length > 0) result += "\r\n" + body;

            return result;
        }
    }
}